#### Defaults ####

# These tags refer to the _Simplified Part-of-Speech Tagset_, as listed in chapter 5 of the nltk book
# http://www.nltk.org/book/ch05.html
non_keyword_tags = ['CNJ','DET','P','NUM','WH']

# a hardcoded set is also accepted
non_keywords = {'is','to','it'}

#tags to ignore for handle_data processing
ignored_tags = ['style','script']

#we only count links inside the text
text_nodes = ['title','div','h1','h2','h3','h4','h5','h6','p']

######################

#TODO:
# a general improvement would be to remove <b> <script> and other tags, to maximize strings and increase size of strings analyzed (hence improving context for tagging with nltk)

import nltk
import networkx
from HTMLParser import HTMLParser
import argparse
import logging
import string


#HTMLParser needs unicode text
non_keyword_tags = map(lambda x: unicode(x),non_keyword_tags)
non_keywords     = map(lambda x: unicode(x),non_keywords)
ignored_tags     = map(lambda x: unicode(x),ignored_tags)
text_nodes       = map(lambda x: unicode(x),text_nodes)


class OurHTMLParser(HTMLParser):
	"""
	The HTML parser. Please read the HTMLParser documentation for more help.
	"""

	def __init__(self):

		HTMLParser.__init__(self)		

		#graph, we store as a list of edges (where each edge is a pair like (node_a,node_b))
		self.graph= []

		#our reverse indexes dictionaries
		self.keyword_idx = {}
		self.nkeyword_idx  = {}

		#the anchors dictionary
		self.anchors = {}

		#our word stemmer
		self.stemmer = nltk.stem.PorterStemmer()

		#we keep track of keyword positions, and all positions
		self.__keyword_pos__ = 0
		self.__abs_pos__     = 0

		#are we inside a url or a to be ignored tag?
		self.__url__    = ''
		self.__ignore__ = False

		#are we inside text?
		self.__qtext__    = False

		#keep track of last relevant tag for current position, for text classification 
		#(ignore <b>, count <h1>-<h6>,<p> etc)
		self.__tag__ = ''



	def handle_starttag(self,tag,attrs):
		"""Action when encountering tag opening."""

		if tag in text_nodes:
			self.__qtext__ = True
			self.__tag__   = tag

		#are we starting a url inside a text node? 
		tag_a = tag is unicode('a')
		text_node = self.__qtext__
		process_url = tag_a and text_node

		#if so, get url from attributes
		if process_url:
			for att in attrs:				
				if att[0] == unicode('href'):
					self.__url__ = att[1]
			# uncomment if desired, too verbose for real-world files
			# if self.__url__ is '':
			# 	#url not found
			# 	logging.warning('No href in tag <a>')
		elif tag in ignored_tags:
			self.__ignore__ = True

	def handle_endtag(self,tag):
		"""Action when encountering tag closing."""

		#ending...
		# a url node?
		if tag is unicode('a'):
			self.__url__ = ''
		# a text tag?
		elif tag in text_nodes:
			self.__qtext__ = False
			self.__tag__   = ''
		# an ignored tag?
		elif tag in ignored_tags:
			self.__ignore__ = False

	def handle_data(self,data):
		"""Processing clear text, our main task"""
		
		not_ignore = not self.__ignore__

		if not_ignore:
			#process url's
			self.process_anchor(data)

			#do reverse indexes
			self.process_words(data)

	def process_anchor(self,data):
		"""Process anchors in data"""

		#process <a> tag if we are inside text nodes and if url exists
		is_text    = self.__qtext__
		url        = self.__url__.encode('utf-8')
		data       = data.encode('utf-8')
		check_anchor = is_text and url

		if check_anchor:
			if data not in self.anchors:
				self.anchors[data]=url

	def process_words(self,s):
		"""Process words in string s"""

		#Removing punctuation from string
		exclude = set(string.punctuation)
		s = ''.join(ch for ch in s if ch not in exclude)

		#converting to lowercase
		s = s.lower()

		#now we want a tagged list like [('They', 'PRP'), ('refuse', 'VBP'),...]
		#we tag entire strings of text, as the tag may depend on the context
		words =  nltk.word_tokenize(s)
		words_tagged = nltk.pos_tag(words)

		#however, the default tagging rules are complex. we use the simplified POS-tagging
		words_tagged = [(word, nltk.tag.simplify.simplify_wsj_tag(tag)) for word, tag in words_tagged]

		#Porter stemming words
		words_tagged = [(self.stemmer.stem(x[0]),x[1]) for x in words_tagged]

		for token in words_tagged:
			#found another word
			word = token[0]
			pos_tag = token[1]
			self.__abs_pos__ += 1
			is_keyword = not(pos_tag in non_keyword_tags or word in non_keywords)

			if is_keyword:
				#found keyword, putting in keyword index
				self.__keyword_pos__ += 1
				self.keyword_idx[self.__keyword_pos__] = {'word':word,'idx':self.__abs_pos__,'tag':self.__tag__}
			else:
				#found non-keyword, putting in non-keyword index
				self.nkeyword_idx[self.__abs_pos__] = {'word':word,'idx':self.__abs_pos__,'tag':self.__tag__}
				


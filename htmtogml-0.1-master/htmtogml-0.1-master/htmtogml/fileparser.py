
from hparser import OurHTMLParser
import os
import json
import networkx as nx

from converttypes import gengraph,idx2json


def fileparser(file_path,graph_path,keyword_path,nkeyword_path,anchor_path):
	"""
	Processes the input html file, saves graph and indexes to input paths.
	"""

	f = open(file_path, 'r')

	#TODO perhaps a sanity check, see if file exists and is well formated, also checking dirs

	#main event
	#feed the file contents to our HTML parser, to process it
	html_code = f.read().decode('utf-8')
	html_parser = OurHTMLParser()
	html_parser.feed(html_code)

	#keep the results
	anchors     = html_parser.anchors
	keyword_idx = html_parser.keyword_idx
	nkeyword_idx   = html_parser.nkeyword_idx

	#get the main file name: /home/user/fileX.html -> fileX
	fullname = os.path.basename(file_path)
	name     = os.path.splitext(fullname)[0]

	#converting graphs and indices to save
	graph        = gengraph(keyword_idx)
	keyword_dic  = idx2json(keyword_idx)
	nkeyword_dic = idx2json(nkeyword_idx)

	graph_filepath     = os.path.join(graph_path,name) + '_graph' + '.gml'
	keyword_filepath  = os.path.join(keyword_path,name) + '_keywords' + '.json'
	nkeyword_filepath = os.path.join(nkeyword_path,name) + '_non_keywords' + '.json'
	anchor_filepath = os.path.join(anchor_path,name) + '_anchors' + '.json'


	nx.write_gml(graph,graph_filepath)
		
	with open(keyword_filepath, 'w') as keyword_f:
		json.dump(keyword_dic,keyword_f)

	with open(nkeyword_filepath, 'w') as nkeyword_f:
		json.dump(nkeyword_dic,nkeyword_f)

	with open(anchor_filepath, 'w') as anchor_f:
		json.dump(anchors,anchor_f)
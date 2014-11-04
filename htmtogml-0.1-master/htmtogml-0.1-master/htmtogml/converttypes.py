import networkx as nx

def gengraph(graph_dict):
	"""Given our internal index format, it returns the networkx graph format."""

	# create networkx graph
	G=nx.Graph()

	nodes = [value['word'] for value in graph_dict.values()]

	for node in nodes:
		G.add_node(node)

	last_item_idx = len(graph_dict)

	#edges are consecutive words (according to their index)
	for i in range(1,last_item_idx):
		word_A = graph_dict[i]['word']
		word_B = graph_dict[i+1]['word']
		G.add_edge(word_A,word_B)

	return G


def idx2json(index_dict):
	"""Given our internal index format, it returns a clean dictionary for the desired json output."""

	out_dict = {}

	for idx,value in index_dict.iteritems():
		word = value['word']
		idx  = value['idx']
		tag  = value['tag']
		
		if word in out_dict:
			out_dict[word].append((tag,idx))
		else:
			out_dict[word]=[(tag,idx)]

	return out_dict
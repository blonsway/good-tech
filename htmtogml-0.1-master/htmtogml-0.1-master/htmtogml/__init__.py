#!/usr/bin/env python

__version__ = '0.1'

#breaking early if libs non-present
import nltk
import networkx as nx
from HTMLParser import HTMLParser
import argparse
import os
from fileparser import fileparser

def main():

	current_dir = os.getcwd()

	#processing user input
	cli_parser = argparse.ArgumentParser(description='Text analysis: reverse word index and word graph generation')

	cli_parser.add_argument('--input','-i', metavar='FILE', type=str, nargs='+',required=True,dest='input_files',
							help='html input files')

	cli_parser.add_argument('--graph','-g', metavar='DIR',type=str, nargs=1,default=current_dir,
							help='Output path to store the generated graphs. Default is current dir')

	cli_parser.add_argument('--keywords','-k', metavar='DIR',type=str, nargs=1,default=current_dir,
							help='Output path to store the generated reverse index(es) for keywords. Default is current dir')

	cli_parser.add_argument('--non-keywords','-n', metavar='DIR',type=str, nargs=1,default=current_dir,dest='nkeywords',
							help='Output path to store the generated reverse index(es) for non-keywords. Default is current dir')

	cli_parser.add_argument('--anchors','-a', metavar='DIR',type=str, nargs=1,default=current_dir,
							help='Output path to store the anchor list file(s). Default is current dir')

	args = cli_parser.parse_args()

	config = {}

	#files and dirs might have been given as relative or absolute paths
	#->put everything in absolute path
	config['input_files']   = map(lambda x: os.path.abspath(x) ,args.input_files)
	config['graph_dir']     = os.path.abspath(args.graph)
	config['keyword_dir']  = os.path.abspath(args.keywords) 
	config['nkeyword_dir'] = os.path.abspath(args.nkeywords) 
	config['anchor_dir']   = os.path.abspath(args.anchors)

	#main loop
	for file_path in config['input_files']:
		fileparser(file_path=file_path,
					graph_path=config['graph_dir'],
					keyword_path=config['keyword_dir'],
					nkeyword_path=config['nkeyword_dir'],
					anchor_path=config['anchor_dir'])
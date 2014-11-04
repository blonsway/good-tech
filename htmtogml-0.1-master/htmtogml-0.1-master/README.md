htmtogml
========

# Note to Good Tech group 

This script was developed by Steve Masiclat for other work, but may serve as a foundation for the Salton Vector analysis.  
GML = Graph Markup Language (for importing into Mathematica)


# Installation

## Requisites

This script was designed in and for Python 2.7 (or later versions of branch 2.x).

In all operating systems, you will find these already included with most Python scientific distributions. I recommend Anaconda.

### Libraries

* nltk
* networkx
* htmlparser (standard lib)
* argparse (standard lib)
* pip (for installation)

Linux distributions come usually bundled with Python, but not the extra libs. Install pip (depends on distribution) and simply run:

``` bash
pip install nltk networkx
```

Alternatively these might be available in your distribution's package repository.

### NLTK add-ons

This library requires a number of internal packages, models and corpora to work properly. After installing ntlk, to make available all the nltk data, run from the shell:

```bash
python -m nltk.downloader all
```

Or select interactively (inside Python) with:

```python
nltk.download()
```

Note that future versions of NLTK and/or of the corpus/models etc might imply changes in the list of keyword types.

## Installing the package

```bash
pip install htmtogml.zip
```

# Usage

Help is provided built-in.

```bash
htmtogml --help
```

# Code overview

Since this package is mostly directed towards students who will need to understand the code, here is a brief overview of the package.

The main file to understand is hparser.py. Essentially we implement an HTMLParser, and to understand that, I strongly recommend reading [an example HTMLParser](https://docs.python.org/2/library/htmlparser.html#example-html-parser-application) for a quick understanding of the HTMLParser mechanism.

What our package does overall is:
* Receives all the HTML files to process (__init__.py);
* Passes each file through the HTML parser above, to get the data we need for the reverse indexes and the graph; 
* Converts our internal data to common formats through converttypes.py (GML for the graph and JSON for the indexes).

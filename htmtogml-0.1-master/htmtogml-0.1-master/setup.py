from distutils.core import setup
setup(
  name = 'htmtogml',
  packages = ['htmtogml'], # this must be the same as the name above
  version = '0.1',

  classifiers=[
    'Development Status :: 3 - Alpha',
    'Intended Audience :: Students',
    'Programming Language :: Python :: 2.7',
  ],

  description = 'A text analysis tool',
  author = 'Nelson Batalha',
  author_email = 'nelson.batalha@gmail.com',
  url = 'https://github.com/n-batalha/htmtogml',
  download_url = 'https://github.com/n-batalha/htmtogml/tarball/0.1', 
  keywords = ['reverse index', 'html', 'word graph'], # arbitrary keywords
  license='MIT',

  install_requires = ['nltk','networkx'],

  entry_points={
        'console_scripts': [
            'htmtogml=htmtogml:main',
        ],
    },
)
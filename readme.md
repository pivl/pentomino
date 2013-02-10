## Pentomino Solver

Simple pentomino puzzle solver made in C# in MonoDevelop based on Donald Knuth's Algorythm X and Dancing Links implementation.

# Description

To read more about the pentomino puzzle refer to:
http://en.wikipedia.org/wiki/Pentomino

The fastest way to find the solution is using the Algorythm X by Donald Knuth's.
To understand how the pentomino puzzle of covering the rectangular area by pentominos is
represented throuth Exact Cover, read
http://en.wikipedia.org/wiki/Exact_cover

Some links to finding solutions to Exact cover:
http://en.wikipedia.org/wiki/Algorithm_X
http://en.wikipedia.org/wiki/Dancing_Links

Some slides explaining Dancing Links implementation:
http://www.cs.colostate.edu/~cs420dl/slides/DLX.pdf

# Usage

`pentamino input_file_name`

input_file_name is the file in the following format:
```
oooooooo
oooooooo
oooooooo
ooo  ooo
ooo  ooo
oooooooo
oooooooo
oooooooo
```
Lowercase letter 'o' is the cell where figure can be placed.
Space ' ' is the cell where figure cannot be placed.

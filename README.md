A simple program to check spelling. Takes lines as dictionary and text to compare them and edit words.

Available edits:

  • Inserting a single letter
  
  • Deleting a single letter

------------------

Restrictions:

  • No more than two edits
  
  • If the edits are both insertions or both deletions, they may not be of adjacent 
  characters.

------------------

Output restrictions:

  • If word is in the dictionary, print it as is
  
  • Otherwise, if word is not in the dictionary:
  
- If no corrections can be found, print “{word?}”;

- Ignore any corrections that require two edits if there is at least one that
requires only one edit;

- If exactly one correction is left, print that word;

- If more than one possible correction is left, print the set of corrections as “{word1
    word2 · · ·}”, in the order they appear in the dictionary.

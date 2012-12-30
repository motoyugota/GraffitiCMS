
====================================
 About the v1.2 Refactoring Branch
====================================

This branch was started as a copy of the early 1.3 branch
around Feb 2010 timeframe. It was intended to be the future
version 2.0 release, and put in the "trunk" folder at first.

In March 2010 Dan Hounshell made extensive (and much needed) refactoring
changes to decouple many components and modernize the codebase. 
The SQL backend was not changed, so no db upgrade scripts were needed.

Unfortunately we split this branch off too early in hindsight.
The 1.3 version still needed a good deal of work and bug fixing, and with
limited available time/resources by all involved, we failed to keep the 
refactoring branch up-to-date. 

In December 2012 the final 1.3 version was remerged into Trunk, to allow
for a possible 2.0 version in the future. This branch can be used as a 
base / reference for refactoring the v2.0 Trunk if desired.


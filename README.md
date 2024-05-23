## Introduction

I needed a tree view for a project I was working on and found that Unity doesn’t natively provide one. So, I decided to create one myself, as it would be a great opportunity to learn more about tree-based data structures.

This is a very basic tree view where each of its nodes has two distinct states: expanded or collapsed. This state determines whether the children of a node are visible or hidden. If the parent is collapsed, then the children are hidden. If the parent is expanded, then the children are visible.

## Research

As the name suggests, a tree view lends itself particularly well to a tree data structure, as nodes have parents and children and are nested within each other.

With this in mind, I started researching tree traversal methods and created several helper functions for that purpose.

While they are not actually used in the project, they are useful in highlighting the types of traversal: depth-first search and breadth-first search, as well as their technical approaches: iteration vs recursion.

Generally speaking, I found that iterative methods are preferable over their recursive counterparts.

### Disadvantages of recursion

- It can lead to a stack overflow as the data is stored on the stack.
- By its very nature, calling a function multiple times can add additional time.
- When debugging, it is harder to follow the program’s execution.

Keep in mind that these points, while valid in theory, will truly impact performance and your ability to debug in the case of very large trees.

### Advantages of iteration

Using an iterative approach, coupled with using a stack in the case of the depth-first search algorithm, addresses all of the previously raised issues:

- The stack is stored on the heap, so there is no risk of stack overflow.
- Only one function call.
- Easier to debug (a for loop is easier to follow than nested function calls).

In light of these advantages, I naturally went for the iterative approach when coding the specific algorithms I needed for the project.

## Algorithms

The user can either expand or collapse the nodes. This involves the following actions:

- Toggling the nodes' visibility.
- Repositioning the nodes below the one that is expanded or collapsed, either up or down.

Both algorithms have O(n) time complexity each. So we go through the tree twice.

### Toggling visibility algorithm

Collapsing is straightforward: we hide every descendant of the starting node; in other words, its children, its children's children, and so on. On the other hand, when expanding, we only show the nodes whose parents are set to “expanded”.

While iterating down, we also count the number of visible nodes along the way. This value is then passed to the repositioning algorithm.

### Repositioning algorithm

We start from the node that the user clicked on. The idea is to move all of the elements below it either up or down by the amount of visible nodes below the starting node. As mentioned before, this is determined by the expanded/collapsed state of each node’s parent.

The program then goes recursively through each parent and moves the nodes below the starting element by this quantity until the root node is reached.


## Potential future features

- Automatically set the parent and children fields based on the Unity hierarchy. 
- Dynamically add or remove nodes
- Automatically position nodes at the start of the game based on the Unity hierarchy
- Function bindings
- Animation

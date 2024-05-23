using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropdownElement : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private DropdownElement parent;

    [SerializeField] private DropdownElement[] children;

    private RectTransform rectTransform;

    private Image image;

    private bool expandState = true;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (children == null || children.Length == 0)
            return;

        if (this.expandState == true)
        {
            Collapse();
        }

        else
        {
            Expand();
        }
    }

    private void Collapse()
    {
        int expandedDescendentsQuantity = 0;

        void ToggleDescendentsVisibility(DropdownElement dropDownElement)
        {
            if (dropDownElement.children == null || dropDownElement.children.Length == 0)
                return;

            var stack = new Stack<DropdownElement>();
            stack.Push(dropDownElement);

            while (stack.Count > 0)
            {
                var currentElement = stack.Pop();

                foreach (var child in currentElement.children)
                {
                    if (currentElement.expandState == true)
                    {
                        expandedDescendentsQuantity++;
                        child.image.enabled = false;
                    }
                    else
                    {
                        child.image.enabled = false;
                    }

                    stack.Push(child);
                }
            }
        }

        ToggleDescendentsVisibility(this);

        this.expandState = false;

        RepositionLowerElements(this, expandedDescendentsQuantity);
    }

    private void Expand()
    {
        this.expandState = true;
        int expandedDescendentsQuantity = 0;

        void ToggleDescendantVisibilityStack(DropdownElement dropDownElement)
        {
            if (dropDownElement.children == null || dropDownElement.children.Length == 0)
                return;

            var stack = new Stack<DropdownElement>();
            stack.Push(dropDownElement);

            while (stack.Count > 0)
            {
                var currentElement = stack.Pop();

                foreach (var child in currentElement.children)
                {
                    if (currentElement.expandState)
                    {
                        expandedDescendentsQuantity++;
                        child.image.enabled = true;
                    }
                    else
                    {
                        child.image.enabled = false;
                    }

                    stack.Push(child);
                }
            }
        }

        ToggleDescendantVisibilityStack(this);

        RepositionLowerElements(this, expandedDescendentsQuantity * -1);
    }


private void RepositionLowerElements(DropdownElement startElement, int rowQuantity)
{
    DropdownElement currentElement = startElement;

    while (currentElement.parent != null)
    {
        int startIndex = Array.IndexOf(currentElement.parent.children, currentElement) + 1;

        for (int i = startIndex; i < currentElement.parent.children.Length; i++)
        {
            currentElement.parent.children[i].MoveBy(rowQuantity);
        }

        currentElement = currentElement.parent;
    }
}

    private void MoveBy(int rows)
    {
        rectTransform.anchoredPosition += new Vector2(0f, rows * 50f);
    }

    // Helper functions
    /*
    private List<DropdownElement> GetDescendants()
    {
        List<DropdownElement> visitedNodes = new List<DropdownElement>();

        void Collect(DropdownElement node)
        {
            foreach (var child in node.children)
            {
                visitedNodes.Add(child);

                Collect(child);
            }
        }

        Collect(this);
        return visitedNodes;
    }
    private void TraverseParent_Recursion()
    {
        if (parent == null)
            return;

        Debug.Log("Traversed parent :" + parent.name);

        parent.TraverseParent_Recursion();
    }

    private void TraverseChildren_Recursion()
    {
        if (children != null && children.Length > 0)
        {
            foreach (DropdownElement child in children)
            {
                Debug.Log(child.gameObject.name);

                child.TraverseChildren_Recursion();
            }
        }
    }
    private List<DropdownElement> GetSiblings()
    {
        List<DropdownElement> siblings = new List<DropdownElement>();

        if (parent != null && parent.children != null)
        {
            for (int i = 0; i < parent.children.Length; i++)
            {
                if (parent.children[i] != this)
                {
                    siblings.Add(parent.children[i]);
                }
            }
        }

        return siblings;
    }

    private void TraverseChildrenDepthFirst_Stack()
    {
        Stack<DropdownElement> stack = new Stack<DropdownElement>();

        if (children == null)
            return;

        for (int i = children.Length - 1; i >= 0; i--)
        {
            stack.Push(children[i]);
        }

        while (stack.Count > 0)
        {
            DropdownElement currentElement = stack.Pop();

            Debug.Log(currentElement.gameObject.name);

            if (currentElement.children == null)
                continue;

            for (int i = currentElement.children.Length - 1; i >= 0; i--)
            {
                stack.Push(currentElement.children[i]);
            }
        }
    }

    private void TraverseChildrenBreadthFirst_Stack()
    {
        Queue<DropdownElement> stack = new Queue<DropdownElement>();

        if (children == null)
            return;

        foreach (DropdownElement child in children)
        {
            stack.Enqueue(child);
        }

        while (stack.Count > 0)
        {
            DropdownElement currentElement = stack.Dequeue();

            Debug.Log(currentElement.gameObject.name);

            if (currentElement.children == null)
                continue;

            foreach (DropdownElement child in currentElement.children)
            {
                stack.Enqueue(child);
            }
        }
    }

    public DropdownElement FindDeepestChild()
    {
        if (children == null || children.Length == 0)
        {
            Debug.LogError("This node has no children, therefore we cannot find its deepest child.");
            return null;
        }

        DropdownElement deepestChild = null; // This will hold the deepest node found
        int maxDepth = -1; // Initialize with -1 to represent that no node has been visited yet

        // Recursive function to traverse the tree and find the deepest node
        void DFS(DropdownElement node, int depth)
        {
            if (node == null) return;

            // If this node is deeper than the current deepest, update deepestChild and maxDepth
            if (depth > maxDepth)
            {
                deepestChild = node;
                maxDepth = depth;
            }

            // Recurse for all children
            foreach (var child in node.children)
            {
                DFS(child, depth + 1);
            }
        }

        // Start the DFS from the current node
        DFS(this, 0); // Depth of the root is 0

        return deepestChild; // Return the deepest node found
    }

    private DropdownElement GetDeepestChild()
    {
        return null;
    }

    private void ToggleVisibilityChildren(bool onOff)
    {
        foreach (DropdownElement child in children)
        {
            child.ToggleVisibilityState(onOff);
            child.UpdateImageVisibility();
        }
    }

    private DropdownElement GetLowestChild()
    {
        DropdownElement currentElement = this;

        if (currentElement.children.Length <= 0)
        {
            Debug.Log("This dropdown element has no children");
            return null;
        }

        while (currentElement.children.Length > 0)
        {
            currentElement = currentElement.children[children.Length - 1];
        }

        return currentElement;
    }

    private List<DropdownElement> GetActiveDescendants()
    {
        List<DropdownElement> visitedNodes = new List<DropdownElement>();

        void Collect(DropdownElement node)
        {
            foreach (var child in node.children)
            {
                visitedNodes.Add(child);

                Collect(child);
            }
        }

        Collect(this);
        return visitedNodes;
    }

    private List<DropdownElement> GetAncestors()
    {
        List<DropdownElement> visitedNodes = new List<DropdownElement>();

        void Collect(DropdownElement node)
        {
            if (node.parent != null)
            {
                visitedNodes.Add(node.parent);

                Collect(node.parent);
            }
        }

        Collect(this);
        return visitedNodes;
    }

    private void TraverseParent_Stack()
    {
        Stack<DropdownElement> stack = new Stack<DropdownElement>();

        if (this.parent != null)
        {
            stack.Push(this.parent);
        }

        while (stack.Count > 0)
        {
            DropdownElement currentElement = stack.Pop();

            Debug.Log("Traversed parent :" + currentElement.name);

            if (currentElement.parent != null)
                stack.Push(currentElement.parent);
        }
    }
    private void TraverseParent_Iteration()
    {
        DropdownElement currentElement = this.parent;

        while (currentElement != null)
        {
            Debug.Log("Traversed parent :" + currentElement.name);

            currentElement = currentElement.parent;
        }
    }
    */

}
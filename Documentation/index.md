# Unity Behavior Tree API

# Functional documentation

## Introduction

This API facilitates the development of tree nodes and offers a visual tool to constitute behaviortrees based on these nodes.

This tool is intuitive to use and can be used by game designers.

## Getting started

First, the developer must create the tree's nodes. 
The abstract class [node](api/BehaviorTree.Node.html) allows for an easy implementation of the node. 
Every class that inherits from Node will be detected in the visual tool.

### Implementing a node

In this case we will implement an action of AI in an infiltration game. 
The wanted behavior is that the AI runs after the player if they are spotted.


#### Create the node

First, we create a class for the desired action which inherits from [Node](api/BehaviorTree.Node.html).

```c#
using BehaviorTree;
using NodeReflection;
using UnityEngine;

namespace Infiltration
{
    [VisualNode] //Makes the class usable in the visual editor
    public class TaskGoTowardEnemy : Node //Inherits 
    {
    
    }
}
```

The [VisualNode] tag lets the visual editor know this is a node which can be used.

#### Adding properties

We can define properties which can be altered in the visual editor with the [ExposedInVisualEditor] tag.
```c#
namespace Infiltration
{
    [VisualNode] 
    public class TaskGoTowardEnemy : Node
    {
        //Properties that are editable in the visual editor inspector.
        [ExposedInVisualEditor]
        public float Speed { get; set; }
        [ExposedInVisualEditor]
        public float UnseeRange { get; set; }
    }
}
```
TODO faire une capture d'écran des propriétés

#### Retrieving agent components

We retrieve the Spriterenderer from the data stored in the AI's gameobject so that we can modify it during the behavior.

The OnInitialize method is called once when the tree is initialized

The OnStart method is called when the status is not executed.

```c#
namespace Infiltration
{
    [VisualNode] 
    public class TaskGoTowardEnemy : Node
    {
        //Properties that are editable in the visual editor inspector.
        [ExposedInVisualEditor]
        public float Speed { get; set; }
        [ExposedInVisualEditor]
        public float UnseeRange { get; set; }
        
        private SpriteRenderer _renderer;

        //Is called once when the tree is initialized
        protected override void OnInitialize()
        {
            this._renderer = this.Agent.GetComponent<GuardSceneData>().FieldOfView;
        }

        //Called on node update if the status is NotExecuted.
        protected override NodeState OnStart()
        {
            _renderer.color = Color.red;

            return NodeState.Running;
        }
        
    }
}
```

#### Adding the verifications and behavior

All that is left to do is to add the verifications and behavior to get this finalized node:
```c#
using BehaviorTree;
using NodeReflection;
using UnityEngine;

namespace Infiltration
    {
    [VisualNode] //Makes the class usable in the visual editor
    public class TaskGoTowardEnemy : Node
    {
        //Properties that are editable in the visual editor inspector.
        [ExposedInVisualEditor]
        public float Speed { get; set; }
        [ExposedInVisualEditor]
        public float UnseeRange { get; set; }
        
        private SpriteRenderer _renderer;

        //Is called once when the tree is initialized
        protected override void OnInitialize()
        {
            this._renderer = this.Agent.GetComponent<GuardSceneData>().FieldOfView;
        }

        //Called on node update if the status is NotExecuted.
        protected override NodeState OnStart()
        {
            _renderer.color = Color.red;

            return NodeState.Running;
        }
        
        //Called on node update if the status is Running.
        protected override NodeState OnUpdate()
        {
            var target = this.GetData<Transform>("target");
            
            //If the player is no longer in range, the action doesn't apply   
            if (Vector3.Distance(this.Agent.transform.position, target.position) > UnseeRange || GameManager.StateSet)
            {
                RemoveData("target");
                _renderer.color = Color.blue;
                return NodeState.Failure;
            }

            //If the player is in range, run the action
            if (Vector3.Distance(this.Agent.transform.position, target.position) > .1f)
            {
                this.Agent.transform.position = Vector3.MoveTowards(
                    this.Agent.transform.position,
                    target.position,
                    Speed * Time.deltaTime
                );
                this.Agent.transform.LookAt(target);
                
                return NodeState.Running;
            }
            
            //The Behavior succeeded.
            return NodeState.Success;
        }
    }
}
```

The node can now easily be used and apply actual behavior in the visaul editor.

## Visual Editor

## [API Developper documentation](api/toc.html)

The documentation can be found [here](api/toc.html)
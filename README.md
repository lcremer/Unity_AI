#AI Behavior Tree Framework

NOTE: this is still very much a Prototype|Proof of Concept

Create Behavior Trees in C#, without having to work with an editor. Preview behavior tree in realtime.
I wanted something that would let me quickly define and create behavior tree nodes how I wanted, something fairly lightweight.
This framework was an attempt to tackle that.

Included is a demo scene BehaviorTest. In the scene is a BehaviorTest prefab.
The prefab has a BehaviorTest (Forgive my naming!) component that gives and example of how to use the framework.
Hitting play will allow you to hit the "Open Behavior Tree Preview" button on the component without errors.

Note: the behavior tree is executed so quickly its best to pause and tick forward to see how the nodes are iterated through.

Pending updates:
Preview without having to hit play.
Tree Execution Speed Adjustment (Tree nodes are acted on so quickly it may be hard to debug)
Collapsable Branches
Node Logging
Utility Curve Selectors

Examples of the Tree Previewer in Unity 5:
https://github.com/lcremer/Unity_AI/blob/master/Misc/BehaviorTree1.PNG
https://github.com/lcremer/Unity_AI/blob/master/Misc/BehaviorTree2.PNG

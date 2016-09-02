# GravityChallenge
Kinect planetary jump game
I made this game for an event at the Cal-Academy. 
It uses kinect tracking to animate astronaut avatars. The idea is to
show how high you might jump on the moon or other planets, with their respective 
gravities factoring into the force of your actual jump.
It uses the vertical motion of the hip joint from the kinect, when the velocity 
increases past a threshold, it adds a directional force to the root node of the 
astronaut avatar, which is a rigid body. The jump is affecetd by the gravity
settings in the scene. 
The game pits two players against each otehr in a race to collect space rocks
by jumping to grab a hook. When the astronaut avatar's hand collides with the hook object, 
the rigid body is turned off so that the astronaut stays parented to the hook, 
unitl it is carried off screen at the top, when the rigid body is turned back on
and the astronaut falls back to the ground, in the speed allowed by that planet's gravity.
Most of the Kinect wrapper and manager files are third-party from the Unity Asset store,
those are in GravityChallenge/Windows/
the Astronaut FBX was downloaded from NASA as were images of the Moon and Mars. 
The images of Venus were from the Venera missions.
I created most of teh materials and shaders.




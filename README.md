Coding problem for SA Interview

## Problem
You will be given a block of square rooms in an X by Y configuration, with a door in the center of every
wall. Some rooms will have a mirror in them at a 45-degree angle. The mirrors may reflect off both
sides (2-way mirrors) or reflect off one side and allow the beam to pass through from the other (1-way
mirrors). When the laser hits the reflective side of one of the mirrors, the beam will reflect off at a 90-
degree angle. Your challenge is to calculate the exit point of a laser shot into one of the maze. You need
to provide the room it will be exiting through along with the orientation. The definition file will be
provided through command line parameters.


## Example File
```
5,4
-1
1,2RR
3,2L
-1
1,0V
-1
```

## Output
The dimensions of the board
The start position of the laser in the format (X, Y) and the orientation (H or V)
The exit point of the laser in the format (X, Y) and the orientation (H or V)
```
Width: 5, Height: 4
StartX: 0, StartY: 2, Laser: H
EndX: 0, EndY: 1, Laser: H
```

# Introduction
Our program is a 3D graphing calculator application. The calculator allows users to model three-dimensional mathematical equations as well as display changing graphs over time. It can be used for exploration or for visualizing data with more than two axes. For specific usage information, see the **usage** section below.

# Installation
To install the program, navigate to the **releases** page (on the right of the github repository) and download the graphing_calculator.exe file of the latest release. Windows Defender may prompt a warning message - click "more info" and then "run anyways."

The program should launch in fullscreen - to exit, you can use **alt** + **f4** to force quit, or close it via the Task Manager with **ctrl** + **shift** + **esc**, or you can simply **alt-tab** out of the application.

# Usage
The calculator app itself has a "guide" button which displays usage instructions, so keep that in mind if you're stuck! For quick graph examples, use the "random" button to generate a random pre-made graph.

## Basic info
Mathematical equations on the calculator are graphed using the ***z(x, y) = \<expression>*** format, similar to a TI-8X's ***y = \<expression>*** feature. This means that you can type in an expression which uses both the **x** and **y** variables. Additionally, by using a **t** variable, the graph will animate based on the current value of **t**, which represents time (in seconds) since the application was opened. To get an oscillating **t** value, plug it into sin() or cos() (see chart below).

## Syntax documentation
|Syntax|Definition|
|--|--|
|+|Add|
|-|Subtract|
|*|Multiply|
|/|Divide|
|sin(**\<expression>**)|Plugs **\<expression>** into a sine function|
|cos(**\<expression>**)|Plugs **\<expression>** into a cosine function|
|tan(**\<expression>**)|Plugs **\<expression>** into a tangent function|
|ln(**\<expression>**)|Evaluates the natural logarithm of **\<expression>**|
|x|[Variable] value of the current point on the graph's x-axis|
|y|[Variable] value of the current point on the graph's y-axis|
|t|[Variable] time (in seconds) since the application was opened. Causes the graph to animate along with the changes in this variable.|

## UI/Controls
|Key|Effect|
|--|--|
|Up/down/left/right-arrows|Rotate camera|
|Shift + Up/down/left/right-arrows|Pan up/down/left/right|
|Shift + Ctrl + Up/down-arrows|Pan forwards/back|
|Ctrl + "="|Zoom in|
|Ctrl + "-"|Zoom out|

|UI Element|Effect|
|--|--|
|*[Enter Equation Here]*|Fill this in with the equation to graph, using **x**, **y**, and/or **t** variables|
|Precision|The balance of accuracy vs. performance cost of the graph. A higher precision value graphs a higher concentration of points, which increases accuracy but may not function on slower hardware. A lower precision value graphs a lower concentration of points, which decreases accuracy but will easily run on slower hardware.|
|Size|The size taken up by the graph. You shouldn't need a size larger than 15 (to reduce size of the actual graph, multiply all your **x** and **y** values by a constant).|
|Graph!|Press this after your settings have been configured to display your graph!|
|Random|Generates a random pre-made equation that looks nice. Great to find cool-looking examples!|

**Thank you!**

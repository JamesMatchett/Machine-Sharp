# Machine-Sharp
## Experimentation in a machine learning paradigm

</hr>

## Machine sharp hopes to be a fully accessible, open source library featuring multiple types of pre-designed although customisable machine learning network styles as well as a base class allowing developers to design their own types of nets and contribute them to our growing library.

</hr> 

## Binary Net update

I've had an idea further to the EFME (Execution flow management Engine) idea that I've had, it's to create a system where each possible answer a neural net can have is itself represented by a binary sub-net (a net with only 1 output, a double which represents how much the binary net's answer matches that to the inputs given to it)

A bit abstract without an example so take MNIST for example, traditionally we have 1 net that has 10 outputs, 0~9 and whichever output is highest we select as our answer, my solution is a little different where we have 10 nets, each net represents a number 0~9, each net is simulated with the same inputs and whichever net is the most positive we then select as our answer.

The advantage of this is that each neuron in the net isn't shoved about 10 different ways for 10 different answers during the training of the neural network, instead it's more binary, either positive to show the inputs match the tested condition (i.e. the picture is of the expected number) or vice versa.

This branch is the preliminary refactoring before binary sub-nets are fully implemented and tested. 

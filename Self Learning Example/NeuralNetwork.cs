using System;
using System.Collections.Generic;

namespace Self_Learning_Example
{
    public class NeuralNetwork
    {
        //define global variables
        public static float learningRate { get; } = 0.05F;
        public List<List<Perceptron>> perceptrons { get; private set; } = new List<List<Perceptron>>();

        //constructor
        public NeuralNetwork()
        {
            //input layer(3 inputs in front of creature)
            perceptrons.Add(new List<Perceptron>() { new Perceptron(0, 4), new Perceptron(0, 4), new Perceptron(0, 4) });

            //hidden layer(4 layers)
            perceptrons.Add(new List<Perceptron>() { new Perceptron(3, 2), new Perceptron(3, 2), new Perceptron(3, 2), new Perceptron(3, 2) });

            //output layer(xOffs and yoffs to move(-1, 1))
            perceptrons.Add(new List<Perceptron>() { new Perceptron(4, 2), new Perceptron(4, 2) });
        }

        //forward propagate
        public float[] ForwardPropagate(float[] inputsToFeed)
        {
            float[] inputs = inputsToFeed;

            //iterate over layers
            for (int layerIndex = 0; layerIndex < perceptrons.Count; layerIndex++)
            {
                List<Perceptron> layer = perceptrons[layerIndex];
                float[] inputsBuffer = new float[layer.Count];

                //iterate over neurons
                for (int neuronIndex = 0; neuronIndex < layer.Count; neuronIndex++)
                {
                    inputsBuffer[neuronIndex] = layer[neuronIndex].output(inputs);
                }

                inputs = inputsBuffer;
            }


            return inputs;
        }

        //backpropagate
        public void BackPropagate(float[] inputsToFeed, float target)
        {
            //forward prop to load error
            ForwardPropagate(inputsToFeed);


        }

    }
}

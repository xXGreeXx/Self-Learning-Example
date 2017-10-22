using System;

namespace Self_Learning_Example
{
    public class Perceptron
    {
        //define global variables
        public float[] weights { get; private set; }
        public int outputs { get; private set; }
        public float error { get; private set; } = 0;
        float bias = 1;

        //constructor
        public Perceptron(int numberOfInputs, int numberOfOutputs)
        {
            //set weight values
            weights = new float[numberOfInputs];

            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = (float)Game.r.NextDouble() * Game.r.Next(-1, 2);
            }

            //define outputs
            outputs = numberOfOutputs;
        }

        //output
        public float output(float[] inputs)
        {
            float answer = 0;

            for (int i = 0; i < weights.Length; i++)
            {
                answer += weights[i] * inputs[i];
            }
            answer += bias;

            return answer;
        }

        //train
        public float train(float[] inputs, float target, Boolean takeTargetAsError)
        {
            float guess = output(inputs);
            float error = takeTargetAsError ? target : target - guess;

            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] += inputs[i] * error * NeuralNetwork.learningRate;
            }

            this.error = error;
            return error;
        }
    }
}

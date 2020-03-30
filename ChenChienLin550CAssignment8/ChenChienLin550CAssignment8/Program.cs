﻿/*******************************************************************************************************
 * Assignment 8 :  ChenChienLin550CAssignment8__Program Class                                          *
 *                                                                                                     *
 * Name:           Chen-Chien Lin                                                                      *
 * Student Number: 46205175                                                                            *
 * Purpose:        Implement the ContinousTimeStateSpaceSystem, StateFeedBack, StateEstimator,         *
 *                 ComplexNumber, and Simulator classes to design and simulate a state feedback        *
 *                 controller and state estimator.                                                     *
 *                                                                                                     *
 * Description:    In this project, other four namespaces are used to demonstrate the process of       *
 *                 designing a controller and state estimator of a state space model.                  *
 *                 ContinousTimeStateSpaceSystem class is created to initiate a state space sysyem.    *
 *                 Instances of ComplexNumber class are the desired pole location of the system.       *
 *                 An instance of StateFeedBack is created to move system poles to desired position.   *
 *                 An instance of StateEstimator is created to estimate states of system.              *
 *                 An instance of Simulator is created to simulate the step response of a system.      *
 *******************************************************************************************************/

/***************************************** USING NAMESPACES ********************************************/

using ChenChienLin550CAssignment8Model;
using ChenChienLin550CAssignment8Simulator;
using ChenChienLin550CAssignment8Controller;
using ChenChienLin550CAssignment8ComplexNumber;

namespace ChenChienLin550CAssignment8
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            double[,] matrixA = new double[2, 2];
            matrixA[0, 0] = 1; matrixA[0, 1] = 0;
            matrixA[1, 0] = 0; matrixA[1, 1] = 1;

            double[,] matrixB = new double[2, 1];
            matrixB[0, 0] = 1; matrixB[1, 0] = 0;

            double[,] matrixC = new double[1, 2];
            matrixC[0, 0] = 1; matrixB[0, 1] = 0;

            double[,] matrixD = new double[1, 1];
            matrixD[0, 0] = 0;

            ContinousTimeStateSpaceSystem ctSystem =
                new ContinousTimeStateSpaceSystem(matrixA, matrixB, matrixC, matrixD);

            // Define sampling time
            double ts = 0.01d;
            // Transfer continuous time system to discrete time system
            DiscreteTimeStateSpaceSystem dtSystem = ctSystem.ToDiscreteTimeSystem(ts);

            // Define a complex number array
            ComplexNumber[] newPoleLocation = new ComplexNumber[matrixA.GetLength(1)];

            // Place new poles to 0.5 + 0.5j and 0.5 - 0.5j
            newPoleLocation[0] = new ComplexNumber(0.5, 0.5);
            newPoleLocation[1] = new ComplexNumber(0.5, -0.5);

            // Create a state feed back controller
            StateFeedBack stateFeedBack = new StateFeedBack();

            // Calculate state feed back gain
            double[,] K = stateFeedBack.PolePlacement(matrixA, matrixB, newPoleLocation);

            // Simulate the closed loop step response
            Simulator simulator = new Simulator();
            simulator.ClosedStepResponse(ctSystem, K);


            // Define a complex number array
            ComplexNumber[] estPoleLocation = new ComplexNumber[matrixA.GetLength(1)];

            // Place poles to 5 + 1j and 5 - 1j
            estPoleLocation[0] = new ComplexNumber(5, 1);
            estPoleLocation[1] = new ComplexNumber(5, -1);

            // Create a state estimator
            StateEstimator stateEstimator = new StateEstimator();

            // Calculate state feed back gain
            double[,] L = stateEstimator.Observer(matrixA, matrixC, estPoleLocation);

            //Simulate the closed loop step response for observer based state feedback system
            simulator.StateFeedBackEstimatorStepResponse(ctSystem, K, L);
        }
    }
}

﻿using System.Collections.Generic;

namespace Ex02
{
    internal class GameDataControl
    {
        private List<string> m_ExistingMovesForCurrentPlayer;
        private bool m_WasExistingMovesChecked = false;

        public bool IsNewMoveValid(GameData i_Data, string i_PlayerMove)
        {
            bool isValidMove = false;
            if (!this.m_WasExistingMovesChecked)
            {
                this.CalcAllExistingMovesForCurrentPlayer(i_Data);
                this.m_WasExistingMovesChecked = true;
            }

            foreach (string existingMove in this.m_ExistingMovesForCurrentPlayer)
            {
                if (i_PlayerMove == existingMove)
                {
                    isValidMove = true;
                }
            }

            return isValidMove;
        }

        public GameData EnterMoveToData(GameData i_Data, string i_Move)
        {
            int row, column;
            row = int.Parse(i_Move[1].ToString()) - 1;
            column = i_Move[0] - 'A';
            if (i_Data.m_PlayerTurn == eTurns.PlayerOneTurn)
            {
                i_Data.m_BoxStatusMatrix[row, column] = eBoxStatuses.PlayerOne;
                i_Data.m_BoxStatusMatrix = convertLeft(row, column, i_Data.m_BoxStatusMatrix, eBoxStatuses.PlayerOne, eBoxStatuses.PlayerTwo);
                i_Data.m_BoxStatusMatrix = convertRight(row, column, i_Data.BoardSize, i_Data.m_BoxStatusMatrix, eBoxStatuses.PlayerOne, eBoxStatuses.PlayerTwo);
                i_Data.m_BoxStatusMatrix = convertUp(row, column, i_Data.m_BoxStatusMatrix, eBoxStatuses.PlayerOne, eBoxStatuses.PlayerTwo);
                i_Data.m_BoxStatusMatrix = convertDown(row, column, i_Data.BoardSize, i_Data.m_BoxStatusMatrix, eBoxStatuses.PlayerOne, eBoxStatuses.PlayerTwo);
            }
            else
            {
                i_Data.m_BoxStatusMatrix[row, column] = eBoxStatuses.PlayerTwo;
                i_Data.m_BoxStatusMatrix = convertLeft(row, column, i_Data.m_BoxStatusMatrix, eBoxStatuses.PlayerTwo, eBoxStatuses.PlayerOne);
                i_Data.m_BoxStatusMatrix = convertRight(row, column, i_Data.BoardSize, i_Data.m_BoxStatusMatrix, eBoxStatuses.PlayerTwo, eBoxStatuses.PlayerOne);
                i_Data.m_BoxStatusMatrix = convertUp(row, column, i_Data.m_BoxStatusMatrix, eBoxStatuses.PlayerTwo, eBoxStatuses.PlayerOne);
                i_Data.m_BoxStatusMatrix = convertDown(row, column, i_Data.BoardSize, i_Data.m_BoxStatusMatrix, eBoxStatuses.PlayerTwo, eBoxStatuses.PlayerOne);
            }
            this.m_WasExistingMovesChecked = false;
            return i_Data;
        }

        public bool AnyExistingMove(GameData i_Data)
        {
            bool isExistingMove = false;
            if (!this.m_WasExistingMovesChecked)
            {
                this.CalcAllExistingMovesForCurrentPlayer(i_Data);
                this.m_WasExistingMovesChecked = true;
            }

            if (this.m_ExistingMovesForCurrentPlayer.Count > 0)
            {
                isExistingMove = true;
            }

            return isExistingMove;
        }

        private static eBoxStatuses[,] convertLeft(int i_I, int i_K, eBoxStatuses[,] i_BoxStatusesMatrix, eBoxStatuses i_CurrentPlayer, eBoxStatuses i_OtherPlayer)
        {
            while (i_K > 0 && i_BoxStatusesMatrix[i_I,i_K - 1] == i_OtherPlayer)
            {
                i_BoxStatusesMatrix[i_I, i_K - 1] = i_CurrentPlayer;
                i_K--;
            }

            return i_BoxStatusesMatrix;
        }

        private static eBoxStatuses[,] convertRight(int i_I, int i_K, int i_BoardSize, eBoxStatuses[,] i_BoxStatusesMatrix, eBoxStatuses i_CurrentPlayer, eBoxStatuses i_OtherPlayer)
        {
            while (i_K < i_BoardSize - 1 && i_BoxStatusesMatrix[i_I, i_K + 1] == i_OtherPlayer)
            {
                i_BoxStatusesMatrix[i_I, i_K + 1] = i_CurrentPlayer;
                i_K++;
            }

            return i_BoxStatusesMatrix;
        }

        private static eBoxStatuses[,] convertUp(int i_I, int i_K, eBoxStatuses[,] i_BoxStatusesMatrix, eBoxStatuses i_CurrentPlayer, eBoxStatuses i_OtherPlayer)
        {
            while (i_I > 0 && i_BoxStatusesMatrix[i_I - 1, i_K] == i_OtherPlayer)
            {
                i_BoxStatusesMatrix[i_I - 1, i_K] = i_CurrentPlayer;
                i_I--;
            }

            return i_BoxStatusesMatrix;
        }

        private static eBoxStatuses[,] convertDown(int i_I, int i_K, int i_BoardSize, eBoxStatuses[,] i_BoxStatusesMatrix, eBoxStatuses i_CurrentPlayer, eBoxStatuses i_OtherPlayer)
        {
            while (i_I < i_BoardSize - 1 && i_BoxStatusesMatrix[i_I + 1, i_K] == i_OtherPlayer)
            {
                i_BoxStatusesMatrix[i_I + 1, i_K] = i_CurrentPlayer;
                i_I++;
            }

            return i_BoxStatusesMatrix;
        }

        private void CalcAllExistingMovesForCurrentPlayer(GameData i_Data)
        {
            if (i_Data.m_PlayerTurn == eTurns.PlayerOneTurn)
            {
                this.m_ExistingMovesForCurrentPlayer = allExistingMoveForCurrentPlayer(i_Data, eBoxStatuses.PlayerOne, eBoxStatuses.PlayerTwo);
            }
            else
            {
                this.m_ExistingMovesForCurrentPlayer = allExistingMoveForCurrentPlayer(i_Data, eBoxStatuses.PlayerTwo, eBoxStatuses.PlayerOne);
            }
        }

        private static List<string> allExistingMoveForCurrentPlayer(GameData i_Data, eBoxStatuses i_CurrentPlayer, eBoxStatuses i_OtherPlayer)
        {
            List<string> allExistingMoves = new List<string>();
            for (int i = 0; i < i_Data.BoardSize; i++)
            {
                for (int k = 0; k < i_Data.BoardSize; k++)
                {
                    if (i_Data.m_BoxStatusMatrix[i, k] == i_OtherPlayer)
                    {
                        // Top, left side of board
                        if (i == 0 && k == 0)
                        {
                            k++;
                        }

                        // Top, right side of board
                        if (i == 0 && k == i_Data.BoardSize - 1)
                        {
                            i++;
                            k = 0;
                        }

                        // Bottom, left side of board
                        if (i == i_Data.BoardSize - 1 && k == 0)
                        {
                            k++;
                        }

                        // Bottom, right side of board
                        if (i == i_Data.BoardSize - 1 && k == i_Data.BoardSize - 1)
                        {
                            continue;
                        }

                        // Top and bottom row
                        if (i == 0 || i == i_Data.BoardSize - 1)
                        {
                            if (isLeftValidMove(i, k, i_Data.BoardSize, i_Data.m_BoxStatusMatrix, i_CurrentPlayer, i_OtherPlayer))
                            {
                                allExistingMoves.Add(convertRowAndColumnToMoves(i, k - 1));
                            }

                            if (isRightValidMove(i, k, i_Data.BoardSize, i_Data.m_BoxStatusMatrix, i_CurrentPlayer, i_OtherPlayer))
                            {
                                allExistingMoves.Add(convertRowAndColumnToMoves(i, k + 1));
                            }
                        }

                        // Right and left column
                        else if (k == 0 || k == i_Data.BoardSize - 1)
                        {
                            if (isUpValidMove(i, k, i_Data.BoardSize, i_Data.m_BoxStatusMatrix, i_CurrentPlayer, i_OtherPlayer))
                            {
                                allExistingMoves.Add(convertRowAndColumnToMoves(i - 1, k));
                            }

                            if (isDownValidMove(i, k, i_Data.BoardSize, i_Data.m_BoxStatusMatrix, i_CurrentPlayer, i_OtherPlayer))
                            {
                                allExistingMoves.Add(convertRowAndColumnToMoves(i + 1, k));
                            }
                        }

                        // Checking all the boxes that or not in the side of the board
                        else
                        {
                            if (isLeftValidMove(i, k, i_Data.BoardSize, i_Data.m_BoxStatusMatrix, i_CurrentPlayer, i_OtherPlayer))
                            {
                                allExistingMoves.Add(convertRowAndColumnToMoves(i, k - 1));
                            }

                            if (isRightValidMove(i, k, i_Data.BoardSize, i_Data.m_BoxStatusMatrix, i_CurrentPlayer, i_OtherPlayer))
                            {
                                allExistingMoves.Add(convertRowAndColumnToMoves(i, k + 1));
                            }

                            if (isUpValidMove(i, k, i_Data.BoardSize, i_Data.m_BoxStatusMatrix, i_CurrentPlayer, i_OtherPlayer))
                            {
                                allExistingMoves.Add(convertRowAndColumnToMoves(i - 1, k));
                            }

                            if (isDownValidMove(i, k, i_Data.BoardSize, i_Data.m_BoxStatusMatrix, i_CurrentPlayer, i_OtherPlayer))
                            {
                                allExistingMoves.Add(convertRowAndColumnToMoves(i + 1, k));
                            }
                        }
                    }
                }
            }

            return allExistingMoves;
        }

        private static string convertRowAndColumnToMoves(int i_I, int i_K)
        {
            char row, column;
            row = (char)('1' + i_I);
            column = (char)('A' + i_K);
            return column.ToString() + row.ToString();
        }

        private static bool isLeftValidMove(int i_I, int i_K, int i_BoardSize, eBoxStatuses[,] i_BoxStatusesMatrix, eBoxStatuses i_CurrentPlayer, eBoxStatuses i_OtherPlayer)
        {
            bool isValidMove = false;
            if (i_BoxStatusesMatrix[i_I, i_K - 1] == eBoxStatuses.Natural)
            {
                do
                {
                    i_K++;
                }
                while (i_K < i_BoardSize - 1 && i_BoxStatusesMatrix[i_I, i_K] == i_OtherPlayer);

                if (i_BoxStatusesMatrix[i_I, i_K] == i_CurrentPlayer)
                {
                    isValidMove = true;
                }
            }

            return isValidMove;
        }

        private static bool isRightValidMove(int i_I, int i_K, int i_BoardSize, eBoxStatuses[,] i_BoxStatusesMatrix, eBoxStatuses i_CurrentPlayer, eBoxStatuses i_OtherPlayer)
        {
            bool isValidMove = false;
            if (i_BoxStatusesMatrix[i_I, i_K + 1] == eBoxStatuses.Natural)
            {
                do
                {
                    i_K--;
                }
                while (i_K > 0 && i_BoxStatusesMatrix[i_I, i_K] == i_OtherPlayer);

                if (i_BoxStatusesMatrix[i_I, i_K] == i_CurrentPlayer)
                {
                    isValidMove = true;
                }
            }

            return isValidMove;
        }

        private static bool isUpValidMove(int i_I, int i_K, int i_BoardSize, eBoxStatuses[,] i_BoxStatusesMatrix, eBoxStatuses i_CurrentPlayer, eBoxStatuses i_OtherPlayer)
        {
            bool isValidMove = false;
            if (i_BoxStatusesMatrix[i_I - 1, i_K + 1] == eBoxStatuses.Natural)
            {
                do
                {
                    i_I++;
                }
                while (i_I < i_BoardSize - 1 && i_BoxStatusesMatrix[i_I, i_K] == i_OtherPlayer);

                if (i_BoxStatusesMatrix[i_I, i_K] == i_CurrentPlayer)
                {
                    isValidMove = true;
                }
            }

            return isValidMove;
        }

        private static bool isDownValidMove(int i_I, int i_K, int i_BoardSize, eBoxStatuses[,] i_BoxStatusesMatrix, eBoxStatuses i_CurrentPlayer, eBoxStatuses i_OtherPlayer)
        {
            bool isValidMove = false;
            if (i_BoxStatusesMatrix[i_I + 1, i_K] == eBoxStatuses.Natural)
            {
                do
                {
                    i_I--;
                }
                while (i_I > 0 && i_BoxStatusesMatrix[i_I, i_K] == i_OtherPlayer);

                if (i_BoxStatusesMatrix[i_I, i_K] == i_CurrentPlayer)
                {
                    isValidMove = true;
                }
            }

            return isValidMove;
        }
    }
}
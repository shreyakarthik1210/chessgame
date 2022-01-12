using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chess
{
    // the following enum allows us to identify a chess piece as black or white
    public enum PlayerType
    {
        BLACK,
        WHITE
    }

    public abstract class AbstractChessPiece
    {
        // the following variables define the AbstractChessPiece

        public String Name;             // "Pawn", "Rook", etc.
        public String Abbreviation;     // "P", "R", etc.

        public PlayerType Player;       // BLACK or WHITE
        public int Col;                 // 0 - 7
        public int Row;                 // 0 - 7
        public bool HasMoved;           // true or false

        // This function should be completed by the student.
        // The AbstractChessPiece initializes the member variables with the
        // provided information.
        public AbstractChessPiece(String newName, String newAbbreviation, PlayerType newPlayer)
        {
            // TODO BY STUDENT
            Name = newName;
            Abbreviation = newAbbreviation;
            Player = newPlayer;
            HasMoved = false;
        }

        // This abstract method is defined but not implemented by AbstractChessPiece.
        // Each derived class will have to implement their own version.
        abstract public bool CanMoveToLocation(int targetCol, int targetRow, ChessBoard gameBoard);

        // This method will be implemented by the student.
        // Given a listed of ChessSquares in the path, determine if the indicated
        // targetRow and column can be reached within the number of steps allowed.
        protected bool CanFollowPath(int targetCol, int targetRow, 
                                     LinkedList<ChessSquare> path, int stepsAllowed)
        {
            if(path.Count == 0)
            {
                return false;
            }
            foreach(ChessSquare square in path)
            {
                stepsAllowed = stepsAllowed - 1;
                if (stepsAllowed < 0)
                {
                    return false;
                }
                if(square.ChessPiece != null)
                {
                    if (square.ChessPiece.Player == Player)
                    {
                        return false;
                    }
                    else if(square.Col == targetCol && square.Row == targetRow)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;    // not yet implemented (final project)!
        }

        // Override the ToString() method to return some nicer description of the piece.
        override public String ToString()
        {

            return Player + " " + Name + " at " + "(" + Row + ", " + Col + ")";
        }
    }


    public class Pawn : AbstractChessPiece
    {

        public Pawn(PlayerType player) : base("Pawn", "\u2659", player)
        {

        }
        public override bool CanMoveToLocation(int targetCol, int targetRow, ChessBoard gameBoard)
        {
            if (canCaptureLocation(targetCol, targetRow, gameBoard))
                return true;

            LinkedList<ChessSquare> path = new LinkedList<ChessSquare>();
            if (Player == PlayerType.WHITE)
            {
                path = gameBoard.GetSquaresUp(Col, Row, targetCol, targetRow);
            }
            else if (Player == PlayerType.BLACK)
            {
                path = gameBoard.GetSquaresDown(Col, Row, targetCol, targetRow);
            }

            if (path.Count() == 0)
            {
                return false;
            }

            int maxMoves = 2;
            if (HasMoved)
                {
                    maxMoves = 1;
                }
            for (int i = 0;i < maxMoves; i++)
            {
                ChessSquare step = path.ElementAt(i);
                if(step.ChessPiece != null)
                {
                    return false;
                }
                if (step.Col == targetCol && step.Row == targetRow)
                {
                    return true;

                }
                
               
            }
            return false;
        }
       
        private bool canCaptureLocation(int targetCol, int targetRow, ChessBoard gameBoard)
        {
            
            ChessSquare targetSquare = gameBoard.GetSquare(targetCol, targetRow);
            if (targetSquare.ChessPiece == null)
            {
                return false;
            }
            if (targetSquare.ChessPiece.Player == Player)
            {
                return false;
            }
            bool validMove = false;
            if(Player==PlayerType.WHITE)
            {
                if( targetRow == Row-1 && (Col==targetCol-1 || Col==targetCol+1))                   
                {
                    validMove = true;
                    
                }
                //(targetCol ==targetSquare.Col+1 || targetCol== targetSquare.Col-1))
            }
            if (Player == PlayerType.BLACK)
            {
                if (Row == targetRow - 1 && (Col == targetCol - 1 || Col == targetCol + 1))
                {
                    validMove = true;

                }
            }
            if (validMove)
                return true;
            else
                return false;

        }
        
        
    }


    public class Rook : AbstractChessPiece
    {

        public Rook(PlayerType player) : base("Rook", "\u2656", player)
        {

        }
        public override bool CanMoveToLocation(int targetCol, int targetRow, ChessBoard gameBoard)
        {
            
            LinkedList<ChessSquare> path = gameBoard.GetStraightSquares(Col, Row, targetCol, targetRow);
            return CanFollowPath(targetCol, targetRow, path, path.Count);
        }
    }

    // This class will be implemented by the student.
    public class Knight : AbstractChessPiece
    {
        // TODO BY STUDENT
        public Knight(PlayerType player) : base("Knight", "\u2658", player)
        {

        }
        public override bool CanMoveToLocation(int targetCol, int targetRow, ChessBoard gameBoard)
        {
            if(Math.Abs(targetRow - Row) == 2 && Math.Abs(targetCol - Col) == 1)
               return true;
            if (Math.Abs(targetRow - Row) == 1 && Math.Abs(targetCol - Col) == 2)
                return true;
            return false;
        }
    }


    public class Bishop : AbstractChessPiece
    {

        public Bishop(PlayerType player) : base("Bishop", "\u2657", player)
        {

        }
        public override bool CanMoveToLocation(int targetCol, int targetRow, ChessBoard gameBoard)
        {
            
            LinkedList<ChessSquare> path = gameBoard.GetDiagonalSquares(Col, Row, targetCol, targetRow);
            return CanFollowPath(targetCol, targetRow, path, path.Count);
        }
    }

    // This class will be implemented by the student.
    public class Queen : AbstractChessPiece
    {
        // TODO BY STUDENT
        public Queen(PlayerType player) : base("Queen", "\u2655", player)
        {

        }
        public override bool CanMoveToLocation(int targetCol, int targetRow, ChessBoard gameBoard)
        {
            
            LinkedList<ChessSquare> path = gameBoard.GetStraightSquares(Col, Row, targetCol, targetRow);
            if (CanFollowPath(targetCol, targetRow, path, path.Count))
            {
                return true;
            }
            else
            {
                LinkedList<ChessSquare> pathdiag = gameBoard.GetDiagonalSquares(Col, Row, targetCol, targetRow);
                if (CanFollowPath(targetCol, targetRow, pathdiag, pathdiag.Count))
                    return true;
            }
            return false;
        }
    }

    public class King : AbstractChessPiece
    {
        public King(PlayerType player) : base("King", "\u2654", player)
        {

        }
        public override bool CanMoveToLocation(int targetCol, int targetRow, ChessBoard gameBoard)
        {
            
            LinkedList<ChessSquare> path = gameBoard.GetStraightSquares(Col, Row, targetCol, targetRow);
            if (CanFollowPath(targetCol, targetRow, path, 1))
            {
                return true;
            }
            else
            {
                LinkedList<ChessSquare> pathdiag = gameBoard.GetDiagonalSquares(Col, Row, targetCol, targetRow);
                if (CanFollowPath(targetCol, targetRow, pathdiag, 1))
                    return true;
            }
            return false;
        }
    }

}

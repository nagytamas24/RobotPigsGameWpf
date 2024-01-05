using RobotPigsGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGTest
{
    [TestClass]
    public class PlayerTest
    {
        private Player _player = null!;

        [TestInitialize]
        public void Initialize()
        {
            _player = new Player(new Position(0, 1), 4, FacingDirection.Right);
        }

        [TestMethod]
        public void TurnRightSuccess()
        {
            _player.Turn(MoveDirection.Right);
            Assert.AreEqual(FacingDirection.Down, _player.FacingDirection);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TurnInvalidDirection()
        {
            _player.Turn(MoveDirection.Forward);

        }

        [TestMethod]
        public void MoveRightSuccess()
        {
            _player.Move(MoveDirection.Forward);
            Assert.AreEqual(new Position(1,1), _player.Position);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MoveOutsideOfPlayArea()
        {
            _player.Move(MoveDirection.Backward);
        }

        [TestMethod]
        public void Punch()
        {
            HashSet<Position> expected = new HashSet<Position>
            {
                new Position(0, 0),
                new Position(1, 0),
                new Position(1, 1),
                new Position(0, 2),
                new Position(1, 2)
            };
            HashSet<Position> affectedPositions = _player.Punch();
            bool areEqual = expected.Count == affectedPositions.Count;
            if (areEqual)
            {
                foreach (var pos in affectedPositions)
                {
                    if (!expected.All(x => affectedPositions.Any(y => x.Equals(y))))
                    {
                        areEqual = false;
                    }
                }
            }

            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void Fire()
        {
            HashSet<Position> expected = new HashSet<Position>
            {
                new Position(1, 1),
                new Position(2, 1),
                new Position(3, 1)
            };
            HashSet<Position> affectedPositions = _player.Fire();
            bool areEqual = expected.Count == affectedPositions.Count;
            if (areEqual)
            {
                foreach (var pos in affectedPositions)
                {
                    if (!expected.All(x => affectedPositions.Any(y => x.Equals(y))))
                    {
                        areEqual = false;
                    }
                }
            }

            Assert.AreEqual(expected.Count, affectedPositions.Count);
            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void HurtNormal()
        {
            _player.Hurt(2);

            Assert.AreEqual(1, _player.Health);
        }

        [TestMethod]
        public void HurtBelowZero()
        {
            _player.Hurt(5);

            Assert.AreEqual(0, _player.Health);
        }
    }
}

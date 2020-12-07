using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL;

namespace Unit_Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Game g = new Game(4);
            string hash = g.Hash_layout();
            g.Unhash(hash);
            Assert.AreEqual(hash, g.Hash_layout());
        }

        [TestMethod]
        public void TestHash()
        {
            Game g = new Game(4);
            Assert.IsNotNull(g.Hash_layout());
        }

        [TestMethod]
        public void TestSolvedMethod1()
        {
            bool ok = true;
            for (int size = 4; size <= 6; size++)
            {
                Game g = new Game(size);
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        g.Layout[i][j] = i * size + j + 1;
                    }
                }
                g.Layout[size-1][size-1] = 0;
                ok = ok && g.Solved();
            }
            Assert.IsTrue(ok);
        }

        [TestMethod]
        public void TestSolvedMethod2()
        {
            bool ok = false;
            for (int size = 4; size <= 6; size++)
            {
                Game g = new Game(size);
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        g.Layout[i][j] = i * size + j + 1;
                    }
                }
                g.Layout[size - 2][size - 2] = 0;
                ok = ok || g.Solved();
            }
            Assert.IsFalse(ok);
        }

        [TestMethod]
        public void TestMoveDownUp()
        {
            Game g = new Game(4);
            string hash = g.Hash_layout();
            g.Move(Moves.Down);
            g.Move(Moves.Up);
            if (g.Turns == 1)
            {
                g.Move(Moves.Down);
            }
            Assert.AreEqual(hash, g.Hash_layout());
        }

        [TestMethod]
        public void TestMoveUpDown()
        {
            Game g = new Game(4);
            string hash = g.Hash_layout();
            g.Move(Moves.Up);
            g.Move(Moves.Down);
            if (g.Turns == 1)
            {
                g.Move(Moves.Up);
            }
            Assert.AreEqual(hash, g.Hash_layout());
        }

        [TestMethod]
        public void TestMoveRightLeft()
        {
            Game g = new Game(4);
            string hash = g.Hash_layout();
            g.Move(Moves.Right);
            g.Move(Moves.Left);
            if (g.Turns == 1)
            {
                g.Move(Moves.Right);
            }
            Assert.AreEqual(hash, g.Hash_layout());
        }

        [TestMethod]
        public void TestMoveLeftRight()
        {
            Game g = new Game(4);
            string hash = g.Hash_layout();
            g.Move(Moves.Left);
            g.Move(Moves.Right);
            if (g.Turns == 1)
            {
                g.Move(Moves.Left);
            }
            Assert.AreEqual(hash, g.Hash_layout());
        }

        [TestMethod]
        public void TestTurns()
        {
            Game g = new Game(4);
            g.Move(Moves.Up);
            g.Move(Moves.Right);
            g.Move(Moves.Down);
            g.Move(Moves.Left);
            int t = g.Turns;
            for (int i = 0; i < 100; i++)
            {
                g.Move(Moves.Up);
                g.Move(Moves.Right);
                g.Move(Moves.Down);
                g.Move(Moves.Left);
            }
            Assert.AreEqual(g.Turns, t+400);
        }

        [TestMethod]
        public void TestMouseMoveControl()
        {
            Game g = new Game(4);
            int x = -1;
            int y = -1;
            for (int i = 0; i < 4; i++)
            {
                if (x < 0)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (g.Layout[i][j] == 0)
                        {
                            x = i;
                            y = j;
                            break;
                        }
                    }
                }
                else
                {
                    break;
                }
            }
            if (x == 0)
            {
                g.Move(x + 1, y);
                x++;
            }
            string hash = g.Hash_layout();
            g.Move(x - 1, y);
            g.Move(x, y);
            Assert.AreEqual(hash, g.Hash_layout());
        }


        [TestMethod]
        public void TestMouseMoveControl2()
        {
            Game g = new Game(4);
            int x = -1;
            int y = -1;
            for (int i = 0; i < 4; i++)
            {
                if (x < 0)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (g.Layout[i][j] == 0)
                        {
                            x = i;
                            y = j;
                            break;
                        }
                    }
                }
                else
                {
                    break;
                }
            }
            if (x == 0)
            {
                g.Move(x + 1, y);
                x++;
            }
            string hash = g.Hash_layout();
            g.Move(x - 1, y);
            Assert.AreNotEqual(hash, g.Hash_layout());
        }


        [TestMethod]
        public void TestSize()
        {
            bool ok = true;
            for (int size = 4; size <= 6; size++)
            {
                Game g = new Game(size);
                ok &= (g.Size == size);
            }
            Assert.IsTrue(ok);
        }
    }
}

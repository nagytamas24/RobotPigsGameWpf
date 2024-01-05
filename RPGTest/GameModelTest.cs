using Moq;
using RobotPigsGame.Model;
using RobotPigsGame.Persistence;

namespace RPGTest
{
    [TestClass]
    public class GameModelTest
    {
        private const int _mapSize = 6;

        private GameModel _model = null!;
        private GameData _mockedGameData = null!;
        private Mock<IRobotPigsDataAccess> _mock = null!;

        [TestInitialize]
        public void Initialize()
        {
            Player p1 = new Player(new Position(2, 2), _mapSize, FacingDirection.Down);
            Player p2 = new Player(new Position(3, 3), _mapSize, FacingDirection.Up);

            _mockedGameData = new GameData(_mapSize, p1, p2);

            _mock = new Mock<IRobotPigsDataAccess>();
            _mock.Setup(mock => mock.LoadAsync(It.IsAny<string>())).Returns(() => Task.FromResult(_mockedGameData));

            _model = new GameModel(_mock.Object);
        }

        [TestMethod]
        public void NewGameMediumMap()
        {
            _model.MapSize = MapSize.Medium;
            _model.NewGame();

            Assert.AreEqual(MapSize.Medium, _model.MapSize);
            Assert.AreEqual(new Position(2, 0), _model.GetP1Pos);
            Assert.AreEqual(new Position(3, 5), _model.GetP2Pos);
        }

        [TestMethod]
        public void NewGameLargeMap()
        {
            _model.MapSize = MapSize.Large;
            _model.NewGame();

            Assert.AreEqual(MapSize.Large, _model.MapSize);
            Assert.AreEqual(new Position(3, 0), _model.GetP1Pos);
            Assert.AreEqual(new Position(4, 7), _model.GetP2Pos);
        }

        [TestMethod]
        public async Task LoadGame()
        {
            await _model.LoadGameAsync(String.Empty);

            Assert.AreEqual(MapSize.Medium, _model.MapSize);
            Assert.AreEqual(6, _model.MapSizeValue);
            Assert.AreEqual(new Position(2, 2), _model.GetP1Pos);
            Assert.AreEqual(new Position(3, 3), _model.GetP2Pos);
        }

        [TestMethod]
        public async Task MovePlayerSuccess()
        {
            await _model.LoadGameAsync(String.Empty);

            Command[] p1Commands = new Command[]
            {
                new Command(CommandType.Move, MoveDirection.Forward)
            };
            Command[] p2Commands = new Command[]
            {
                new Command(CommandType.Skip)
            };

            _model.ExecuteCommands(p1Commands, p2Commands);

            Assert.AreEqual(new Position(2, 3), _model.GetP1Pos);
            Assert.AreEqual(new Position(3, 3), _model.GetP2Pos);
        }

        [TestMethod]
        public async Task MovePlayerObstructed()
        {
            await _model.LoadGameAsync(String.Empty);

            Command[] p1Commands = new Command[]
            {
                new Command(CommandType.Move, MoveDirection.Forward),
                new Command(CommandType.Move, MoveDirection.Left)
            };
            Command[] p2Commands = new Command[]
            {
                new Command(CommandType.Skip),
                new Command(CommandType.Skip)
            };

            _model.ExecuteCommands(p1Commands, p2Commands);

            Assert.AreEqual(new Position(2, 3), _model.GetP1Pos);
            Assert.AreEqual(new Position(3, 3), _model.GetP2Pos);
        }

        [TestMethod]
        public async Task MoveBothSuccess()
        {
            await _model.LoadGameAsync(String.Empty);

            Command[] p1Commands = new Command[]
            {
                new Command(CommandType.Move, MoveDirection.Forward)
            };
            Command[] p2Commands = new Command[]
            {
                new Command(CommandType.Move, MoveDirection.Forward)
            };
            _model.ExecuteCommands(p1Commands, p2Commands);

            Assert.AreEqual(new Position(2, 3), _model.GetP1Pos);
            Assert.AreEqual(new Position(3, 2), _model.GetP2Pos);
        }

        [TestMethod]
        public async Task MoveBothObstructed()
        {
            await _model.LoadGameAsync(String.Empty);

            Command[] p1Commands = new Command[]
            {
                new Command(CommandType.Move, MoveDirection.Forward)
            };
            Command[] p2Commands = new Command[]
            {
                new Command(CommandType.Move, MoveDirection.Left)
            };
            _model.ExecuteCommands(p1Commands, p2Commands);

            Assert.AreEqual(new Position(2, 2), _model.GetP1Pos);
            Assert.AreEqual(new Position(3, 3), _model.GetP2Pos);
        }

        [TestMethod]
        public async Task P1Won()
        {
            await _model.LoadGameAsync(String.Empty);

            Command[] p1Commands = new Command[]
            {
                new Command(CommandType.Punch),
                new Command(CommandType.Punch),
                new Command(CommandType.Punch)
            };
            Command[] p2Commands = new Command[]
            {
                new Command(CommandType.Skip),
                new Command(CommandType.Skip),
                new Command(CommandType.Skip)
            };
        }

        public void Game_P1Won(object? sender, int e)
        {
            Assert.AreEqual(1, e);
        }
    }
}
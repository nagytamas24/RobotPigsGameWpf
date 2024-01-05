namespace RobotPigsGame.Persistence
{
    /// <summary>
    /// Data access interface for dependency injection.
    /// </summary>
    public interface IRobotPigsDataAccess
    {
        Task<GameData> LoadAsync(string path);
        Task SaveAsync(string path, GameData gameData);
    }
}
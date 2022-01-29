
namespace AutumnYard.ProjectParry
{
    public interface IPlayerMover
    {
        void Update(in PlayerInputs inputs);
        void Stop();
    }
}

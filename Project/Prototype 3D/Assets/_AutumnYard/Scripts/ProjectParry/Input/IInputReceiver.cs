
namespace AutumnYard.ProjectParry
{
    public interface IInputReceiver
    {
        void UpdateWithInputs(in PlayerInputs inputs);
        void Stop();
    }
}

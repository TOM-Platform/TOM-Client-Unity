public interface IProgressIndicatorObject
{
    void StartProgressIndicator(string message);

    void UpdateProgressIndicator(string message);

    void StopProgressIndicator(string message);
}

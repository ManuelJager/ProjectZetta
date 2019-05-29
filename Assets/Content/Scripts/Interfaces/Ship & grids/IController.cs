public interface IController
{
    void SetThrustVectors(float[] thrustVectors);
    float[] GetThrustVectors();
    void SetTurningRateVectors(float [] turningRateVectors);
    float[] GetTurningRateVectors();
}

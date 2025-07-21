using UnityEngine;

public class MobileInputHandler : MonoBehaviour
{
    public CarMovement car;

    public void GasDown() => car.verticalInput = 1f;
    public void GasUp() => car.verticalInput = 0f;

    public void BrakeDown() => car.verticalInput = -1f;
    public void BrakeUp() => car.verticalInput = 0f;

    public void SteerLeftDown() => car.horizontalInput = -1f;
    public void SteerLeftUp() => car.horizontalInput = 0f;

    public void SteerRightDown() => car.horizontalInput = 1f;
    public void SteerRightUp() => car.horizontalInput = 0f;
}

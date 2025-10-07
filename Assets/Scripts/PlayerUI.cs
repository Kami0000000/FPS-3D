using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform jetpackFuellFill;

    private PlayerController controller;


    public void SetController(PlayerController _controller )
    {
        controller = _controller;
    }


private void Update()
{
SetFuellAmount(controller.GetJetpackFuelAmount());
}
    void SetFuellAmount(float _amount)
    {
        jetpackFuellFill.localScale = new Vector3(1f, _amount, 1f);
    }
}

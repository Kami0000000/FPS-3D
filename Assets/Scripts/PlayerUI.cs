using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform jetpackFuellFill;

    private PlayerController controller;


    [SerializeField]
    private GameObject pauseMenu;

  [SerializeField]
   private GameObject scoreBoard;

    public void SetController(PlayerController _controller )
    {
        controller = _controller;
    }


    private void Start()
    {
        PauseMenu.isOn = false;
    }


private void Update()
{
    
SetFuellAmount(controller.GetJetpackFuelAmount());

if(Input.GetKeyDown(KeyCode.Escape))
{
    TogglePauseMenu();
   
}

if(Input.GetKeyDown(KeyCode.Tab))
{
    scoreBoard.SetActive(true);
}else if(Input.GetKeyUp(KeyCode.Tab))
{
    scoreBoard.SetActive(false); 
}

}
public void TogglePauseMenu()
{
    pauseMenu.SetActive(!pauseMenu.activeSelf);//différent de son sattut actuel
    PauseMenu.isOn = pauseMenu.activeSelf;
}
    void SetFuellAmount(float _amount)
    {
        jetpackFuellFill.localScale = new Vector3(1f, _amount, 1f);
    }
}

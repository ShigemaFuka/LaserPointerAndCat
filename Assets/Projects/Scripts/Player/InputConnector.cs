using UnityEngine;
/// <summary>
/// 入力とロジックを繋げる
/// </summary>
public class InputConnector : MonoBehaviour
{
    [SerializeField] private InputAdapter _inputAdapter;
    [SerializeField] private PlayerController _playerController;
    private void Start()
    {
        _playerController.SetInput(_inputAdapter);
    }
}

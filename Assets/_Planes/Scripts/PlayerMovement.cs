using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f; //скорость
    Transform _player; //позиция игрока
    Transform _nose; //нос - точка, которая находится спереди самолета, и к которой он будет "стремиться"
    FixedJoystick _joystick; //джойстик - управление
    float horizontalMove;
    float verticalMove;

    void Awake()
    {
        _player = GetComponent<Transform>();
        _joystick = GameObject.FindWithTag("Joystick").GetComponent<FixedJoystick>();
        _nose = GameObject.FindWithTag("Nose").GetComponent<Transform>();
    }
    
    void FixedUpdate()
    {
        //постоянное движение вперед
        _player.position = Vector3.MoveTowards(_player.position, _nose.position, speed * Time.deltaTime);

        //выясняем, куда потянут джойстик
        horizontalMove = _joystick.Horizontal;
        verticalMove = _joystick.Vertical;

        if (horizontalMove < -0.2f || horizontalMove > 0.2f) //если джойстик потянут горизонтально на некоторую значительную величину
        {
            _player.Rotate(0, horizontalMove, 0); //вращаем игрока; вместе с ним вращается "нос", в сторону которого он летит
        }

        if (verticalMove < -0.2f || verticalMove > 0.2f) //если джойстик потянут вертикально на некоторую значительную величину
        {
            _player.Rotate(0, 0, verticalMove); //вращаем игрока; вместе с ним вращается "нос", в сторону которого он летит
        }
    }
}

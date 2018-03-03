using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

//Walk forward for an amount of time then disappear 
public class Decoy : MonoBehaviour
{
    public float _moveSpeed;
    public CharacterController _controller;
    public Vector3 _moveDirection;
    public float _gravityScale;
    public float _aliveTime;

    [SpaceAttribute]
    public Animator _anim;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        //start a coroutine where after x amount of seconds the object will hide or kill itself
        StartCoroutine(Kill(_aliveTime));
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    //Controls the movement of the decoy
    void Movement()
    {
        // anim.SetFloat("BlendX", controller.velocity.x);
        // anim.SetFloat("BlendY", controller.velocity.z);
        _moveDirection.y = _moveDirection.y + (Physics.gravity.y * _gravityScale * Time.deltaTime); //apply gravity
        _controller.Move(_moveDirection *  _moveSpeed * Time.deltaTime); //move decoy
    }

    IEnumerator Kill(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            Destroy(this.gameObject);
        }
    }
}

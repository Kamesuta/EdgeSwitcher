using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerAnimator : MonoBehaviour
{
    private Player player;
    public Transform baseObject;
    public Transform spriteObject;
    public float rotateSpeed;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        // 回転
        spriteObject.Rotate(Vector3.forward, -player.turnRight * rotateSpeed * player.speed * Time.deltaTime);

        // 向き(player.moveDirection)から回転(angle)に変換
        float angle = Mathf.Atan2(player.moveDirection.y, player.moveDirection.x) * Mathf.Rad2Deg;

        // 向きを変える
        baseObject.transform.localScale = new Vector3(1, Mathf.Lerp(baseObject.transform.localScale.y, player.turnRight, 0.1f), 1);
        baseObject.transform.localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(baseObject.transform.localEulerAngles.z, angle, 0.3f));
    }
}

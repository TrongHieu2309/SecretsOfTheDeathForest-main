using UnityEngine;

    public class PlayerWallSlideState : PlayerBaseState
    {
        public override void EnterState(PlayerStateManager player)
        {
            player.Anim.SetBool("wallSlide", player.onWall);
        }

        public override void UpdateState(PlayerStateManager player)
        {
            if (!player.isGrounded && player.onWall)
            {
                player.Rb.linearVelocity = new Vector2(player.Rb.linearVelocity.x, Mathf.Clamp(player.Rb.linearVelocity.y, -player.wallSlideSpeed, float.MaxValue));
                player.Anim.SetBool("wallSlide", player.onWall);
                
                if (Input.GetKeyDown(KeyCode.Space) && player.onWall)
                {
                    player.isWallJumping = true;
                    player.Anim.SetBool("wallSlide", player.onWall);

                    float direction = player.IsFacingRight ? -1 : 1;
                    Vector2 wallJumpForce = new Vector2(direction * player.wallJumpHorizontalForce, player.wallJumpForce);
                    player.Rb.AddForce(wallJumpForce, ForceMode2D.Impulse);

                    if (player.IsFacingRight && direction == -1 || !player.IsFacingRight && direction == 1)
                        player.Flip(player.transform);

                    player.StartCoroutine(player.WallJumpCooldown());
                }
            }

            if (!player.onWall)
            {
                player.Anim.SetBool("wallSlide", player.onWall);
            }

            if (player.isGrounded)
            {
                player.SwitchState(player.IdleState);
            }
        }

        public override void ExitState(PlayerStateManager player)
        {
            player.Rb.gravityScale = 5;
            Debug.Log("Exit Wall Jump State");
        }
    }
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterTower : Turret
{

    protected override void RotateTowardsTarget()
    {
        // Get the direction to the target
        Vector2 direction = target.position - transform.position;

        // Calculate the angle from the direction vector
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Clamp the angle between -20 and 20 degrees for aiming
        float clampedAngle = Mathf.Clamp(angle, -20f, 20f);

        // Adjust the clamped angle based on the direction the character is facing
        if (direction.x < 0)
        {
            // Invert the clamped angle when facing left
            clampedAngle = -clampedAngle;
        }

        // Determine the target rotation for the turret
        Quaternion targetRotation = Quaternion.Euler(0, 0, clampedAngle);

        // Smoothly rotate the turret towards the target rotation
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Flip the character on the X-axis if needed
        if (direction.x < 0)
        {
            // Flip the sprite when facing left
            turretRotationPoint.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            // Ensure the sprite is not flipped when facing right
            turretRotationPoint.localScale = new Vector3(1, 1, 1);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyAndDash : MonoBehaviour
{
     public Player player;
    public CheckController checkC;

    #region VOL VAR
    [Header("VOL")]
    [SerializeField] public bool is_flying = false;
    [SerializeField] public bool aerial = false;
    [SerializeField] public bool MadeAFly = false;
    [SerializeField] public float speedVertFly;
    [SerializeField] public float speedHoriFly;
    [SerializeField] public float timerStamina;
    [Range(0, 15)][SerializeField] float gravityfall;
    [HideInInspector] public float smoothFly = 0.06f;
    private bool Govol = false;
    private bool flyactivated = false;


    [Header("STAMINA")]
    public float maxStamina;
    public float currentStamina;
    [SerializeField] StaminaBar staminaBar;
    //private bool Recharging = false;
    #endregion

    #region DASH VAR
    [Header("DASH")]
    [SerializeField] private float dashingPower; //puissance du dash actuel
    [SerializeField] private float dashingCooldown;  // temps du dash de base
    [SerializeField] private float dashingBeforeFlyCooldown; //temps du dash juste avant le vol
    private Vector2 dashingdirection;
    [HideInInspector] public bool canDash = true;
    [HideInInspector] public bool isDashing = false;
    [SerializeField] private float DashingPowerDash; // puissance du dash dans un dash
    [SerializeField] private float dashingPowerVol;  // puissance du dash juste avant le vol ( petite impulsion avant le vol)
    [SerializeField] public TrailRenderer tr;
    #endregion

    void Start()
    {
        maxStamina = timerStamina;
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
    }

    
    void Update()
    {
        /* joystick button 0 = carré dualsense et A
         * joystick button 1 = croix dualsense et B
         * joystick button 2 = rond dualsense et X 
         * joystick button 3 = triangle dualsense et Y
         */
        #region VOL IF
        // verification si on est sur xbox ou ps4 dans un autre script pour passer d'un input a un autre
        if (Input.GetButton(checkC.inputVol) && is_flying == false && aerial && MadeAFly == false)
        {
            player.animController.SetBool("Jumping", true);
            dashingPower = dashingPowerVol;
            player.releasejump = false;
            player.duringJump = false;
            is_flying = true;
            flyactivated = true;
            
            
            Dash();
            StartCoroutine(StopDashing());
            Invoke("Vol", 0.25f);
        }
        if (Input.GetButtonDown(checkC.inputVol) && is_flying == true) ChuteVol();

        // On commence a consumer la barre
        if (is_flying && currentStamina > 0)
        {
            StartBar();
        }
        // lorsque la barre est à 0 on lance la fin du vol
        if (currentStamina == 0 )
        {
            ChuteVol();
        }
        #endregion

        #region DASH IF
       
        if (Input.GetButtonDown(checkC.inputDash) && canDash)
        {
            dashingPower = DashingPowerDash;
            Dash();
            StartCoroutine(StopDashing());

        }

        if (isDashing)
        {
            player.rb.velocity = dashingdirection.normalized * dashingPower;
            return;
        }

        #endregion
    }

    private void FixedUpdate()
    {
        //Stopper le vol si le player stop les mouvements pour eviter de flotter immobile
        if ((player.horizontal_value == 0 && player.vertical_value == 0) && is_flying)
        {

            ChuteVol();
        }
    }

    #region FONCTION ENUM
    #region Bar
    void StartBar()
    {
        timerStamina -= Time.deltaTime;

        if (timerStamina > 0)
        {
            currentStamina -= Time.deltaTime;
            staminaBar.SetStamina(currentStamina);
        }
        if (timerStamina < 0)
        {
            timerStamina = 0;
            currentStamina = 0;
        }
        if(currentStamina < 0)
        {
            currentStamina = 0;
        }
    }
    public void RegenBar()
    {
        timerStamina = 4;
        if (currentStamina < maxStamina)
        {
            currentStamina += Time.deltaTime * 2.5f;
            staminaBar.SetStamina(currentStamina);
        }
        if (currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
        }
    }
    #endregion
    private void Dash()
    {
        isDashing = true;
        canDash = false;
        tr.emitting = true;
        dashingdirection = new Vector2(player.horizontal_value, player.vertical_value);

        if(is_flying == false && dashingdirection == Vector2.zero)
        {
            dashingdirection = new Vector2(transform.localScale.x, y: 0);
        }
    }
    private IEnumerator StopDashing()
    {
        if (flyactivated)
        {
            yield return new WaitForSeconds(dashingBeforeFlyCooldown);
            isDashing = false;
            Govol = true;
        }
        else
        {
            yield return new WaitForSeconds(dashingCooldown);
            if (is_flying == false)
            {
                tr.emitting = false;
            }
            isDashing = false;
            Govol = true;
        }
    }


    public void Vol()
    {
        canDash = true;
        DashingPowerDash = 50;
        MadeAFly = true;
        is_flying = true;
        //animController.SetBool("Aerial", true);
        // animController.SetBool("Jumping", false);
        player.rb.gravityScale = 0;
        player.moveSpeed_vertical = speedVertFly;
        player.moveSpeed_horizontal = speedHoriFly;
    }
    public void ChuteVol()
    {
        DashingPowerDash = 30;
        is_flying = false;
        flyactivated = false;
        tr.emitting = false;
        Govol = false;
        // animController.SetBool("Aerial", false);
        player.moveSpeed_vertical = 400f;
        player.moveSpeed_horizontal = 550f;
        player.rb.gravityScale = gravityfall;

    }
#endregion
}

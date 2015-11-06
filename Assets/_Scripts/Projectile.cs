using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	[SerializeField]
	 private Weapon.WeaponType _type;

	 //This public property masks the field _type & takes action when it is set
	public Weapon.WeaponType type{
		get{
			return(_type);
		}set{
			SetType(value);
	 }
	 }

	 void Awake(){
		//Test to see whether this has passed off screen every 2 seconds
		InvokeRepeating ("CheckOffscreen", 2f, 2f);
	 }

	public void SetType(Weapon.WeaponType eType){
		//Set the type
		_type = eType;
		Weapon.WeaponDefinition def = Main.GetWeaponDefinition (_type);
		renderer.material.color = def.projectileColor;

	}

	void CheckOffscreen(){
		if (Utils.ScreenBoundsCheck (collider.bounds, BoundsTest.offScreen) != Vector3.zero) {
			Destroy(this.gameObject);
		}

	}




}

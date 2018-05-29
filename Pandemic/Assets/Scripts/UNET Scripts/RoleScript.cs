using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PandemicEnum;

[System.Serializable]
public class RoleScript {
	
	private Role roleName;
	private RoleAction[] roleActions;

	// GETTERS & SETTERS

	// Returns the Role to caller
	public Role getRoleName () {
		return this.roleName;
	}

	// Sets the roleName and roleActions
	public void setRole (Role role) {
		switch (role) {
			case Role.ContingencyPlanner:
				this.roleName = role;
				this.roleActions = new RoleAction[] {RoleAction.ContingencyStoreEventCard};
				break;

			case Role.Dispatcher:
				this.roleName = role;
				roleActions = new RoleAction[] {RoleAction.DispatcherMoveOtherPawn};
				break;

			case Role.Medic:
				this.roleName = role;
				break;

			case Role.OperationsExpert:
				this.roleName = role;
				roleActions = new RoleAction[] {RoleAction.OperationsExpertMoveFromRS};
				break;

			case Role.QuarantineSpecialist:
				this.roleName = role;
				break;

			case Role.Researcher:
				this.roleName = role;
				break;

			case Role.Scientist:
				this.roleName = role;
				break;

			default:
				break;
		}
		
	}

}
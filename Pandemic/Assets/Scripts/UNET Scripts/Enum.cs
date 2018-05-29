using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PandemicEnum {
	public enum DiseaseColour {
		Yellow = 0, 
		Red = 1, 
		Blue = 2, 
		Black = 3, 
		Purple = 4
	};

	public enum Role {ContingencyPlanner, Dispatcher, Medic, OperationsExpert, QuarantineSpecialist, Researcher, Scientist};
	public enum RoleAction {DispatcherMoveOtherPawn, ContingencyStoreEventCard, OperationsExpertMoveFromRS};
    public enum EventCard {OneQuietNight, Forecast, GovernmentGrant, Airlift, ResilientPopulation};
}
	
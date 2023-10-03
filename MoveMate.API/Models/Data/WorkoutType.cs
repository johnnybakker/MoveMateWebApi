using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using MoveMate.Models;

namespace MoveMate.Models.Data;


public enum WorkoutType {
	RUNNING = 1,
	WALKING = 2
}

public static class WorkoutTypeExtensions {
	public static EnumEntity<WorkoutType> ToEntity(this WorkoutType value) {
		return EnumEntity<WorkoutType>.GetEntity(value);
	} 
}
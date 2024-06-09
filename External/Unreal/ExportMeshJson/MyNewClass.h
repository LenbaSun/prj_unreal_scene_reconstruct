// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "MyNewClass.generated.h"

UCLASS(Blueprintable)
class PRJ_SCIFI_HALLWAY_API AMyNewClass : public AActor
{
	GENERATED_BODY()
	
public:	
	// Sets default values for this actor's properties
	AMyNewClass();

public:	

	UFUNCTION(BlueprintCallable, Category = "MyBlueprintFunctionLib")
	void ExportModelsToJSON();
};

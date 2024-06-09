#include "MyNewClass.h"
#include "Engine/World.h"
#include "GameFramework/Actor.h"
#include "Kismet/GameplayStatics.h"
#include "Misc/FileHelper.h"
#include "Misc/Paths.h"
#include "Serialization/JsonWriter.h"
#include "Serialization/JsonSerializer.h"
#include "Engine/StaticMeshActor.h"
#include "Components/StaticMeshComponent.h"
#include "Editor/UnrealEd/Public/ObjectTools.h" // For GetActorLabel

// Sets default values
AMyNewClass::AMyNewClass()
{
    // Set this actor to call Tick() every frame. You can turn this off to improve performance if you don't need it.
    PrimaryActorTick.bCanEverTick = true;
}

void AMyNewClass::ExportModelsToJSON()
{
    TArray<AActor*> FoundActors;
    UGameplayStatics::GetAllActorsOfClass(GetWorld(), AStaticMeshActor::StaticClass(), FoundActors);

    TArray<TSharedPtr<FJsonValue>> JsonArray;

    for (AActor* Actor : FoundActors)
    {
        FVector Location = Actor->GetActorLocation();
        FRotator Rotation = Actor->GetActorRotation();
        FVector Scale = Actor->GetActorScale3D();
        FString ActorName = Actor->GetActorLabel();  // 使用 GetActorLabel() 获取 Outliner 中的名称

        // Unreal 坐标系转换到 Unity 坐标系，且单位从厘米转换为米
        FVector UnityLocation = FVector(Location.X / 100.0f, Location.Z / 100.0f, -Location.Y / 100.0f);
        FRotator UnityRotation = FRotator(-Rotation.Pitch, Rotation.Yaw, -Rotation.Roll); // 调整旋转方向
        FVector UnityScale = FVector(Scale.X, Scale.Z, Scale.Y); // 只交换 Y 和 Z，不进行单位转换

        // 获取静态网格组件
        AStaticMeshActor* StaticMeshActor = Cast<AStaticMeshActor>(Actor);
        if (StaticMeshActor && StaticMeshActor->GetStaticMeshComponent())
        {
            UStaticMesh* StaticMesh = StaticMeshActor->GetStaticMeshComponent()->GetStaticMesh();
            if (StaticMesh)
            {
                FString MeshName = StaticMesh->GetName();

                TSharedPtr<FJsonObject> ActorObject = MakeShareable(new FJsonObject);

                ActorObject->SetStringField(TEXT("ActorName"), ActorName);
                ActorObject->SetStringField(TEXT("MeshName"), MeshName);
                ActorObject->SetNumberField(TEXT("PositionX"), UnityLocation.X);
                ActorObject->SetNumberField(TEXT("PositionY"), UnityLocation.Y);
                ActorObject->SetNumberField(TEXT("PositionZ"), UnityLocation.Z);
                ActorObject->SetNumberField(TEXT("RotationPitch"), UnityRotation.Pitch);
                ActorObject->SetNumberField(TEXT("RotationYaw"), UnityRotation.Yaw);
                ActorObject->SetNumberField(TEXT("RotationRoll"), UnityRotation.Roll);
                ActorObject->SetNumberField(TEXT("ScaleX"), UnityScale.X);
                ActorObject->SetNumberField(TEXT("ScaleY"), UnityScale.Y);
                ActorObject->SetNumberField(TEXT("ScaleZ"), UnityScale.Z);

                JsonArray.Add(MakeShareable(new FJsonValueObject(ActorObject)));
            }
        }
    }

    TSharedPtr<FJsonObject> RootObject = MakeShareable(new FJsonObject);
    RootObject->SetArrayField(TEXT("Actors"), JsonArray);

    FString OutputString;
    TSharedRef<TJsonWriter<>> Writer = TJsonWriterFactory<>::Create(&OutputString);
    FJsonSerializer::Serialize(RootObject.ToSharedRef(), Writer);

    FFileHelper::SaveStringToFile(OutputString, *FPaths::Combine(FPaths::ProjectDir(), TEXT("SceneExport.json")));
}

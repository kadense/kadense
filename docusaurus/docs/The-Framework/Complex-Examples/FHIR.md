---
title: FHIR
sidebar_position: 1
---

So far, most of the examples we've given have been very simple, but let's say we want to deal with something a bit more complicated. 

The following demonstrates a limited FHIR R4 sample that has multiple use cases.

Please note that this is not a full implementation of the FHIR standard, but there is no reason why you couldn't implement a full version of the FHIR standard as well.

```yaml
apiVersion: kadense.io/v1
kind: MalleableModule
metadata:
  name: fhir-r4-test
  namespace: default
spec:
  classes:
    Element:
      properties:
        id:
          type: string
          description: The unique identifier for the element.
    Reference:
      'baseClass:': Element
      properties:
        reference:
          type: string
          description: A reference to another resource.
        type:
          type: string
          description: The type of the referenced resource.
        identifier:
          type: Identifier
          description: An identifier for the referenced resource.
        display:
          type: string
          description: Text alternative to the resource reference.
    Coding:
      properties:
        system:
          type: string
          description: The system that defines the code.
        version:
          type: string
          description: The version of the system.
        code:
          type: string
          description: The code from the system.
        display:
          type: string
          description: A human-readable name for the code.
        userSelected:
          type: bool
          description: Indicates if this code was selected by the user.
    CodeableConcept:
      properties:
        coding:
          description: Code defined by a terminology system
          type: array
          subType: Coding
        text:
          type: string
          description: Plain text representation of the concept.
    Metadata:
      properties:
        versionId:
          type: string
          description: The version of the resource.
        lastUpdated:
          type: string
          format: dateTime
          description: The last time the resource was updated.
        source:
          type: string
          description: The source of the resource.
        profile:
          type: array
          description: A list of profiles that the resource conforms to.
          subType: string
        security:
          type: array
          description: Security labels applied to the resource.
          subType: Coding
        tag:
          type: array
          description: Tags applied to the resource.
          subType: Coding
    Period:
      baseClass: Element
      properties:
        start:
          type: string
          format: dateTime
          description: The start of the period.
        end:
          type: string
          format: dateTime
          description: The end of the period.
    Identifier:
      baseClass: Element
      properties:
        use:
          type: string
          description: The purpose of the identifier.
        type:
          type: CodeableConcept
          description: The type of the identifier.
        system:
          type: string
          description: The namespace for the identifier.
        value:
          type: string
          description: The value of the identifier.
        period:
          type: Period
          description: Time period when the identifier is valid.
        assigner:
          type: Reference
          description: Organization that issued the identifier.
    Resource:
      baseClass: Element
      discriminatorProperty: resourceType
      properties:
        meta:
          type: Metadata
          description: Metadata about the resource.
        implicitRules:
          type: string
          description: A set of rules under which the resource was created.
        language:
          type: string
          description: The base language in which the resource is written.
    Bundle:
      baseClass: Resource
      typeDiscriminator: Bundle
      properties:
        type:
          type: string
          description: The type of the bundle.
        entry:
          type: array
          description: Entries in the bundle.
          subType: Entry
    Entry:
      properties:
        fullUrl:
          type: string
          description: The full URL of the entry.
        resource:
          type: Resource
          description: The resource in the entry.
    BackboneElement:
      baseClass: Element
      properties:
        modifierExtension:
          type: array
          description: Extensions that cannot be ignored even if unrecognized.
          subType: Extension
    StatusHistory:
      baseClass: BackboneElement
      properties:
        status:
          type: string
          description: The status of the resource.
        period:
          type: Period
          description: The period during which the status was applicable.
    ClassHistory:
      baseClass: BackboneElement
      properties:
        class:
          type: Coding
          description: The class of the resource.
        period:
          type: Period
          description: The period during which the class was applicable.
    Participant:
      baseClass: BackboneElement
      properties:
        type:
          type: array
          description: The type of participant.
          subType: CodeableConcept
        period:
          type: Period
          description: The period during which the participant was involved.
        individual:
          type: Reference
          description: The individual involved in the encounter.
    Diagnosis:
      baseClass: BackboneElement
      properties:
        condition:
          type: Reference
          description: The condition being diagnosed.
        use:
          type: CodeableConcept
          description: The use of the diagnosis.
        rank:
          type: int
          description: The rank of the diagnosis.
    Hospitalization:
      baseClass: BackboneElement
      properties:
        preAdmissionIdentifier:
          type: Identifier
          description: Pre-admission identifier.
        origin:
          type: CodeableConcept
          description: The origin of the hospitalization.
        admitSource:
          type: CodeableConcept
          description: The source of the admission.
        reAdmission:
          type: CodeableConcept
          description: Indicates if this is a readmission.
        dietPreference:
          type: array
          description: Diet preferences for the patient.
          subType: CodeableConcept
        specialCourtesy:
          type: array
          description: Special courtesies for the patient.
          subType: CodeableConcept
        specialArrangement:
          type: array
          description: Special arrangements for the patient.
          subType: CodeableConcept
        destination:
          type: Reference
          description: The destination of the hospitalization.
        dischargeDisposition:
          type: CodeableConcept
          description: ''
    Location:
      baseClass: BackboneElement
      properties:
        location:
          type: Reference
          description: The location of the encounter.
        status:
          type: string
          description: The status of the location.
        physicalType:
          type: CodeableConcept
          description: The physical type of the location.
        period:
          type: Period
          description: The period during which the location was applicable.
    Encounter:
      baseClass: Resource
      typeDiscriminator: Encounter
      properties:
        identifier:
          type: array
          subType: Identifier
          description: The unique identifier for the encounter.
        status:
          type: string
          description: The status of the encounter.
        statusHistory:
          type: array
          description: History of the encounter status.
          subType: StatusHistory
        classHistory:
          type: array
          description: History of the encounter class.
          subType: ClassHistory
        type:
          type: array
          description: The type of the encounter.
          subType: CodeableConcept
        serviceType:
          type: array
          subType: CodeableConcept
          description: The type of service for the encounter.
        priority:
          type: array
          subType: CodeableConcept
          description: The priority of the encounter.
        subject:
          type: Reference
          description: ''
        episodeOfCare:
          type: array
          description: Episode(s) of care that this encounter should be recorded against
          subType: Reference
        basedOn:
          type: array
          description: Plans or proposals that this encounter is based on.
          subType: Reference
        participant:
          type: array
          description: The participants involved in the encounter.
          subType: Participant
        appointment:
          type: array
          description: The appointment(s) that scheduled this encounter.
          subType: Reference
        period:
          type: Period
          description: The period during which the encounter occurred.
        length:
          description: The length of the encounter.
        reasonCode:
          type: array
          description: The reason for the encounter.
          subType: CodeableConcept
        reasonReference:
          type: array
          description: The reason for the encounter.
          subType: Reference
        diagnosis:
          type: array
          description: The diagnosis for the encounter.
          subType: Diagnosis
        account:
          type: array
          description: The account(s) associated with the encounter.
          subType: Reference
        hospitalization:
          type: Hospitalization
          description: Details about the hospitalization.
        location:
          type: array
          description: The location(s) of the encounter.
          subType: Location
        serviceProvider:
          type: Reference
          description: The service provider for the encounter.
        partOf:
          type: Reference
          description: The encounter that this encounter is part of.
    Patient:
      baseClass: DomainResource
      typeDiscriminator: Patient
      discriminatorClass: Resource
      properties:
        identifier:
          type: array
          description: The unique identifier for the patient.
          subType: Identifier
    EpisodeOfCare:
      baseClass: DomainResource
      typeDiscriminator: EpisodeOfCare
      discriminatorClass: Resource
      properties:
        identifier:
          type: array
          description: The unique identifier for the episode of care.
          subType: Identifier
        status:
          type: string
          description: The status of the episode of care.
        statusHistory:
          type: array
          description: History of the episode of care status.
          subType: StatusHistory
        type:
          type: array
          description: The type of the episode of care.
          subType: CodeableConcept
        diagnosis:
          type: array
          description: The list of diagnosis relevant to this episode of care
          subType: Diagnosis
        patient:
          type: Reference
          description: The patient who this episode of care is for.
        managingOrganization:
          type: Reference
          description: The organization that manages this episode of care.
        period:
          type: Period
          description: The period during which the episode of care is active.
        referralRequest:
          type: array
          description: The referral request(s) that this episode of care is based
            on.
          subType: Reference
        careManager:
          type: Reference
          description: The care manager for the episode of care.
        team:
          type: array
          description: The team members involved in the episode of care.
          subType: Reference
        account:
          type: array
          description: The account(s) associated with the episode of care.
          subType: Reference
    Extension:
      baseClass: Element
      properties:
        url:
          type: string
          description: The URL that identifies the extension.
        value:
          type: string
          description: The value of the extension as a string.
    DomainResource:
      baseClass: Resource
      discriminator: DomainResource
      properties:
        text:
          type: Narrative
          description: Text summary of the resource.
        contained:
          type: array
          description: Resources contained in this resource.
          subType: Resource
        extension:
          type: array
          description: Additional content defined by implementations.
          subType: Extension
        modifierExtension:
          type: array
          description: Extensions that cannot be ignored even if unrecognized.
          subType: Extension
    Narrative:
      baseClass: Element
      properties:
        status:
          type: string
          description: The status of the narrative.
        div:
          type: string
          description: The actual narrative content.
```

This can be interpreted using the following examples (courtesy of HAPI):

```json
{
  "resourceType": "Bundle",
  "id": "9c994981-9e90-4110-9a2f-c87b1f51054a",
  "meta": {
    "lastUpdated": "2022-09-17T08:02:14.990+00:00"
  },
  "type": "searchset",
  "total": 274,
  "link": [
    {
      "relation": "self",
      "url": "https://hapi.fhir.org/baseR4/EpisodeOfCare?_format=json&_pretty=true"
    },
    {
      "relation": "next",
      "url": "https://hapi.fhir.org/baseR4?_getpages=9c994981-9e90-4110-9a2f-c87b1f51054a&_getpagesoffset=20&_count=20&_format=json&_pretty=true&_bundletype=searchset"
    }
  ],
  "entry": [
    {
      "fullUrl": "https://hapi.fhir.org/baseR4/EpisodeOfCare/7002248",
      "resource": {
        "resourceType": "EpisodeOfCare",
        "id": "7002248",
        "meta": {
          "versionId": "1",
          "lastUpdated": "2022-09-13T13:45:06.713+00:00",
          "source": "#dIWZ70WfNoZgdhXJ",
          "tag": [
            {
              "system": "http://terminology.hl7.org/CodeSystem/v3-ActReason",
              "code": "HTEST",
              "display": "More health data"
            }
          ]
        },
        "text": {
          "status": "generated",
          "div": "<div xmlns=\"http://www.w3.org/1999/xhtml\">\n      HACC Program for Peter James Chalmers at HL7 Healthcare 15 Sept 2014 - current<br/>\n\t\t\twas on leave from 22 Sept - 24 Sept while in respite care\n    </div>"
        },
        "identifier": [
          {
            "system": "http://example.org/sampleepisodeofcare-identifier",
            "value": "123"
          }
        ],
        "status": "active",
        "statusHistory": [
          {
            "status": "planned",
            "period": {
              "start": "2014-09-01",
              "end": "2014-09-14"
            }
          },
          {
            "status": "active",
            "period": {
              "start": "2014-09-15",
              "end": "2014-09-21"
            }
          },
          {
            "status": "onhold",
            "period": {
              "start": "2014-09-22",
              "end": "2014-09-24"
            }
          },
          {
            "status": "active",
            "period": {
              "start": "2014-09-25"
            }
          }
        ],
        "type": [
          {
            "coding": [
              {
                "system": "http://terminology.hl7.org/CodeSystem/episodeofcare-type",
                "code": "hacc",
                "display": "Home and Community Care"
              }
            ]
          }
        ],
        "diagnosis": [
          {
            "role": {
              "coding": [
                {
                  "system": "http://terminology.hl7.org/CodeSystem/diagnosis-role",
                  "code": "CC",
                  "display": "Chief complaint"
                }
              ]
            },
            "rank": 1
          }
        ],
        "patient": {
          "reference": "Patient/example"
        },
        "managingOrganization": {
          "reference": "Organization/example"
        },
        "period": {
          "start": "2014-09-01"
        },
        "referralRequest": [
          {
            "display": "Referral from Example Aged Care Services"
          }
        ],
        "careManager": {
          "reference": "Practitioner/1",
          "display": "Amanda Assigned"
        },
        "team": [
          {
            "reference": "CareTeam/example",
            "display": "example care team"
          }
        ],
        "account": [
          {
            "reference": "Account/example",
            "display": "example account"
          }
        ]
      },
      "search": {
        "mode": "match"
      }
    },
    {
      "fullUrl": "https://hapi.fhir.org/baseR4/EpisodeOfCare/6991538",
      "resource": {
        "resourceType": "EpisodeOfCare",
        "id": "6991538",
        "meta": {
          "versionId": "1",
          "lastUpdated": "2022-09-08T11:02:14.607+00:00",
          "source": "#q9v48Z64rn33wU54"
        },
        "contained": [
          {
            "resourceType": "Patient",
            "id": "1",
            "text": {
              "status": "generated",
              "div": "<div xmlns=\"http://www.w3.org/1999/xhtml\"><div class=\"hapiHeaderText\">Nancy Ann <b>KNUDSEN </b></div><table class=\"hapiPropertyTable\"><tbody><tr><td>Identifier</td><td>2512489996</td></tr></tbody></table></div>"
            },
            "identifier": [
              {
                "use": "official",
                "system": "urn:oid:1.2.208.176.1.2",
                "value": "2512489996"
              }
            ],
            "name": [
              {
                "family": "Knudsen",
                "given": [
                  "Nancy",
                  "Ann"
                ]
              }
            ],
            "telecom": [
              {
                "system": "other",
                "value": "NemSMS"
              },
              {
                "system": "other",
                "value": "NemSMS"
              }
            ]
          }
        ],
        "status": "active",
        "patient": {
          "reference": "#1"
        },
        "period": {
          "start": "2021-01-01T00:00:00+01:00"
        }
      },
      "search": {
        "mode": "match"
      }
    },
    {
      "fullUrl": "https://hapi.fhir.org/baseR4/EpisodeOfCare/6991536",
      "resource": {
        "resourceType": "EpisodeOfCare",
        "id": "6991536",
        "meta": {
          "versionId": "1",
          "lastUpdated": "2022-09-08T11:01:49.599+00:00",
          "source": "#2vkp2phW0nalQQRq"
        },
        "contained": [
          {
            "resourceType": "Patient",
            "id": "1",
            "text": {
              "status": "generated",
              "div": "<div xmlns=\"http://www.w3.org/1999/xhtml\"><div class=\"hapiHeaderText\">Nancy Ann <b>KNUDSEN </b></div><table class=\"hapiPropertyTable\"><tbody><tr><td>Identifier</td><td>2512489996</td></tr></tbody></table></div>"
            },
            "identifier": [
              {
                "use": "official",
                "system": "urn:oid:1.2.208.176.1.2",
                "value": "2512489996"
              }
            ],
            "name": [
              {
                "family": "Knudsen",
                "given": [
                  "Nancy",
                  "Ann"
                ]
              }
            ],
            "telecom": [
              {
                "system": "other",
                "value": "NemSMS"
              },
              {
                "system": "other",
                "value": "NemSMS"
              }
            ]
          }
        ],
        "status": "active",
        "patient": {
          "reference": "#1"
        },
        "period": {
          "start": "2021-01-01T00:00:00+01:00"
        }
      },
      "search": {
        "mode": "match"
      }
    },
    {
      "fullUrl": "https://hapi.fhir.org/baseR4/EpisodeOfCare/6991534",
      "resource": {
        "resourceType": "EpisodeOfCare",
        "id": "6991534",
        "meta": {
          "versionId": "1",
          "lastUpdated": "2022-09-08T11:00:53.845+00:00",
          "source": "#NkvSp4jUMIsll5i5"
        },
        "contained": [
          {
            "resourceType": "Patient",
            "id": "1",
            "text": {
              "status": "generated",
              "div": "<div xmlns=\"http://www.w3.org/1999/xhtml\"><div class=\"hapiHeaderText\">Nancy Ann <b>KNUDSEN </b></div><table class=\"hapiPropertyTable\"><tbody><tr><td>Identifier</td><td>2512489996</td></tr></tbody></table></div>"
            },
            "identifier": [
              {
                "use": "official",
                "system": "urn:oid:1.2.208.176.1.2",
                "value": "2512489996"
              }
            ],
            "name": [
              {
                "family": "Knudsen",
                "given": [
                  "Nancy",
                  "Ann"
                ]
              }
            ],
            "telecom": [
              {
                "system": "other",
                "value": "NemSMS"
              },
              {
                "system": "other",
                "value": "NemSMS"
              }
            ]
          }
        ],
        "status": "active",
        "patient": {
          "reference": "#1"
        },
        "period": {
          "start": "2021-01-01T00:00:00+01:00"
        }
      },
      "search": {
        "mode": "match"
      }
    },
    {
      "fullUrl": "https://hapi.fhir.org/baseR4/EpisodeOfCare/6991529",
      "resource": {
        "resourceType": "EpisodeOfCare",
        "id": "6991529",
        "meta": {
          "versionId": "1",
          "lastUpdated": "2022-09-08T10:54:16.280+00:00",
          "source": "#jL2KfWkmu7Tac1mQ"
        },
        "contained": [
          {
            "resourceType": "Patient",
            "id": "1",
            "text": {
              "status": "generated",
              "div": "<div xmlns=\"http://www.w3.org/1999/xhtml\"><div class=\"hapiHeaderText\">Nancy Ann <b>KNUDSEN </b></div><table class=\"hapiPropertyTable\"><tbody><tr><td>Identifier</td><td>2512489996</td></tr></tbody></table></div>"
            },
            "identifier": [
              {
                "use": "official",
                "system": "urn:oid:1.2.208.176.1.2",
                "value": "2512489996"
              }
            ],
            "name": [
              {
                "family": "Knudsen",
                "given": [
                  "Nancy",
                  "Ann"
                ]
              }
            ],
            "telecom": [
              {
                "system": "other",
                "value": "NemSMS"
              },
              {
                "system": "other",
                "value": "NemSMS"
              }
            ]
          }
        ],
        "status": "active",
        "patient": {
          "reference": "#1"
        },
        "period": {
          "start": "2021-01-01T00:00:00+01:00"
        }
      },
      "search": {
        "mode": "match"
      }
    },
    {
      "fullUrl": "https://hapi.fhir.org/baseR4/EpisodeOfCare/6991341",
      "resource": {
        "resourceType": "EpisodeOfCare",
        "id": "6991341",
        "meta": {
          "versionId": "1",
          "lastUpdated": "2022-09-08T10:47:30.481+00:00",
          "source": "#wkFwPOS8YituCZ18"
        },
        "contained": [
          {
            "resourceType": "Patient",
            "id": "1",
            "text": {
              "status": "generated",
              "div": "<div xmlns=\"http://www.w3.org/1999/xhtml\"><div class=\"hapiHeaderText\">Nancy Ann <b>KNUDSEN </b></div><table class=\"hapiPropertyTable\"><tbody><tr><td>Identifier</td><td>2512489996</td></tr></tbody></table></div>"
            },
            "identifier": [
              {
                "use": "official",
                "system": "urn:oid:1.2.208.176.1.2",
                "value": "2512489996"
              }
            ],
            "name": [
              {
                "family": "Knudsen",
                "given": [
                  "Nancy",
                  "Ann"
                ]
              }
            ],
            "telecom": [
              {
                "system": "other",
                "value": "NemSMS"
              },
              {
                "system": "other",
                "value": "NemSMS"
              }
            ]
          }
        ],
        "status": "active",
        "patient": {
          "reference": "#1"
        },
        "period": {
          "start": "2021-01-01T00:00:00+01:00"
        }
      },
      "search": {
        "mode": "match"
      }
    },
    {
      "fullUrl": "https://hapi.fhir.org/baseR4/EpisodeOfCare/6990715",
      "resource": {
        "resourceType": "EpisodeOfCare",
        "id": "6990715",
        "meta": {
          "versionId": "1",
          "lastUpdated": "2022-09-08T09:53:57.631+00:00",
          "source": "#gSduIzJWaBAWBvJr"
        },
        "contained": [
          {
            "resourceType": "Patient",
            "id": "1",
            "text": {
              "status": "generated",
              "div": "<div xmlns=\"http://www.w3.org/1999/xhtml\"><div class=\"hapiHeaderText\">Nancy Ann <b>KNUDSEN </b></div><table class=\"hapiPropertyTable\"><tbody><tr><td>Identifier</td><td>2512489996</td></tr></tbody></table></div>"
            },
            "identifier": [
              {
                "use": "official",
                "system": "urn:oid:1.2.208.176.1.2",
                "value": "2512489996"
              }
            ],
            "name": [
              {
                "family": "Knudsen",
                "given": [
                  "Nancy",
                  "Ann"
                ]
              }
            ],
            "telecom": [
              {
                "system": "other",
                "value": "NemSMS"
              },
              {
                "system": "other",
                "value": "NemSMS"
              }
            ]
          }
        ],
        "status": "active",
        "patient": {
          "reference": "#1"
        }
      },
      "search": {
        "mode": "match"
      }
    },
    {
      "fullUrl": "https://hapi.fhir.org/baseR4/EpisodeOfCare/6990713",
      "resource": {
        "resourceType": "EpisodeOfCare",
        "id": "6990713",
        "meta": {
          "versionId": "1",
          "lastUpdated": "2022-09-08T09:53:35.237+00:00",
          "source": "#63Idqi7EHJ0WpsOd"
        },
        "contained": [
          {
            "resourceType": "Patient",
            "id": "1",
            "text": {
              "status": "generated",
              "div": "<div xmlns=\"http://www.w3.org/1999/xhtml\"><div class=\"hapiHeaderText\">Nancy Ann <b>KNUDSEN </b></div><table class=\"hapiPropertyTable\"><tbody><tr><td>Identifier</td><td>2512489996</td></tr></tbody></table></div>"
            },
            "identifier": [
              {
                "use": "official",
                "system": "urn:oid:1.2.208.176.1.2",
                "value": "2512489996"
              }
            ],
            "name": [
              {
                "family": "Knudsen",
                "given": [
                  "Nancy",
                  "Ann"
                ]
              }
            ],
            "telecom": [
              {
                "system": "other",
                "value": "NemSMS"
              },
              {
                "system": "other",
                "value": "NemSMS"
              }
            ]
          }
        ],
        "status": "active",
        "patient": {
          "reference": "#1"
        }
      },
      "search": {
        "mode": "match"
      }
    },
    {
      "fullUrl": "https://hapi.fhir.org/baseR4/EpisodeOfCare/RT-HHA-EpisodeOfCare",
      "resource": {
        "resourceType": "EpisodeOfCare",
        "id": "RT-HHA-EpisodeOfCare",
        "meta": {
          "versionId": "1",
          "lastUpdated": "2022-06-23T05:16:45.402+00:00",
          "source": "#EKo784v57PlNuUrZ"
        },
        "status": "finished",
        "type": [
          {
            "coding": [
              {
                "system": "http://terminology.hl7.org/CodeSystem/episodeofcare-type",
                "code": "hacc"
              }
            ]
          }
        ],
        "diagnosis": [
          {
            "condition": {
              "reference": "Condition/RT-HHA-Diagnosis-CISequelae"
            }
          }
        ],
        "patient": {
          "reference": "Patient/RT-Patient-BSJ"
        },
        "managingOrganization": {
          "reference": "Organization/RT-PractitionerOrg-SkyHarbor-HHA"
        },
        "period": {
          "start": "2020-02-10",
          "end": "2020-06-29"
        },
        "careManager": {
          "reference": "PractitionerRole/RT-PractitionerRole-RN-JacobHartwell"
        }
      },
      "search": {
        "mode": "match"
      }
    },
    {
      "fullUrl": "https://hapi.fhir.org/baseR4/EpisodeOfCare/RT-SNF-EpisodeOfCare",
      "resource": {
        "resourceType": "EpisodeOfCare",
        "id": "RT-SNF-EpisodeOfCare",
        "meta": {
          "versionId": "1",
          "lastUpdated": "2022-06-23T05:16:45.402+00:00",
          "source": "#EKo784v57PlNuUrZ"
        },
        "status": "finished",
        "type": [
          {
            "coding": [
              {
                "system": "http://terminology.hl7.org/CodeSystem/episodeofcare-type",
                "code": "pac"
              }
            ]
          }
        ],
        "diagnosis": [
          {
            "condition": {
              "reference": "Condition/RT-SNF-Diagnosis-CI"
            }
          }
        ],
        "patient": {
          "reference": "Patient/RT-Patient-BSJ"
        },
        "managingOrganization": {
          "reference": "Organization/RT-PractitionerOrg-HappyNursing-SNF"
        },
        "period": {
          "start": "2020-01-01",
          "end": "2020-02-10"
        },
        "careManager": {
          "reference": "PractitionerRole/RT-PractitionerRole-RN-LiaNguyen"
        }
      },
      "search": {
        "mode": "match"
      }
    },
    {
      "fullUrl": "https://hapi.fhir.org/baseR4/EpisodeOfCare/10001-A-39607008-EC",
      "resource": {
        "resourceType": "EpisodeOfCare",
        "id": "10001-A-39607008-EC",
        "meta": {
          "versionId": "1",
          "lastUpdated": "2022-06-09T09:07:21.055+00:00",
          "source": "#PboMJCNJxLAqNJxY"
        },
        "status": "active",
        "type": [
          {
            "coding": [
              {
                "system": "http://snomed.info/sct",
                "code": "39607008",
                "display": "Lung structure (body structure)"
              },
              {
                "system": "http://snomed.info/sct",
                "code": "39607008",
                "display": "Lung structure (body structure)"
              }
            ],
            "text": "Lung structure (body structure)"
          }
        ],
        "diagnosis": [
          {
            "condition": {
              "reference": "Condition/2947-730402179-C"
            },
            "role": {
              "coding": [
                {
                  "system": "http://hl7.org/fhir/diagnosis-role",
                  "code": "AD",
                  "display": "Admission diagnosis"
                },
                {
                  "system": "http://hl7.org/fhir/diagnosis-role",
                  "code": "AD",
                  "display": "Admission diagnosis"
                }
              ],
              "text": "Admission diagnosis"
            }
          }
        ],
        "patient": {
          "reference": "Patient/10001-A"
        },
        "period": {
          "start": "2017-03-22T00:00:00Z"
        }
      },
      "search": {
        "mode": "match"
      }
    },
    {
      "fullUrl": "https://hapi.fhir.org/baseR4/EpisodeOfCare/2561819",
      "resource": {
        "resourceType": "EpisodeOfCare",
        "id": "2561819",
        "meta": {
          "versionId": "1",
          "lastUpdated": "2021-09-22T12:40:16.804+00:00",
          "source": "#w6RFsuBsb0ieaRS6"
        },
        "identifier": [
          {
            "system": "http://example.org/sampleepisodeofcare-identifier",
            "value": "123"
          }
        ],
        "status": "planned",
        "type": [
          {
            "coding": [
              {
                "system": "http://hl7.org/fhir/episodeofcare-type",
                "code": "hacc",
                "display": "Home and Community Care"
              }
            ]
          }
        ],
        "diagnosis": [
          {
            "condition": {
              "reference": "Condition/88580"
            },
            "role": {
              "coding": [
                {
                  "system": "http://hl7.org/fhir/diagnosis-role",
                  "code": "CC",
                  "display": "Chief complaint"
                }
              ]
            },
            "rank": 1
          }
        ],
        "patient": {
          "reference": "Patient/2561818"
        },
        "managingOrganization": {
          "reference": "Organization/88547"
        },
        "period": {
          "start": "2021-09-08T18:30:00Z",
          "end": "2021-09-09T18:30:00Z"
        }
      },
      "search": {
        "mode": "match"
      }
    },
    {
      "fullUrl": "https://hapi.fhir.org/baseR4/EpisodeOfCare/2561814",
      "resource": {
        "resourceType": "EpisodeOfCare",
        "id": "2561814",
        "meta": {
          "versionId": "1",
          "lastUpdated": "2021-09-22T11:51:41.813+00:00",
          "source": "#spMkc5CkMxe7QKN8"
        },
        "identifier": [
          {
            "system": "http://example.org/sampleepisodeofcare-identifier",
            "value": "123"
          }
        ],
        "status": "active",
        "type": [
          {
            "coding": [
              {
                "system": "http://hl7.org/fhir/episodeofcare-type",
                "code": "pac",
                "display": "Post Acute Care"
              }
            ]
          }
        ],
        "diagnosis": [
          {
            "condition": {
              "reference": "Condition/88580"
            },
            "role": {
              "coding": [
                {
                  "system": "http://hl7.org/fhir/diagnosis-role",
                  "code": "CC",
                  "display": "Chief complaint"
                }
              ]
            },
            "rank": 1
          }
        ],
        "patient": {
          "reference": "Patient/2561813"
        },
        "managingOrganization": {
          "reference": "Organization/88547"
        },
        "period": {
          "start": "2021-09-21T07:00:00Z",
          "end": "2021-09-24T07:00:00Z"
        }
      },
      "search": {
        "mode": "match"
      }
    },
    {
      "fullUrl": "https://hapi.fhir.org/baseR4/EpisodeOfCare/2562548",
      "resource": {
        "resourceType": "EpisodeOfCare",
        "id": "2562548",
        "meta": {
          "versionId": "1",
          "lastUpdated": "2021-09-23T07:10:14.149+00:00",
          "source": "#EF8XHrGRkEJZ2rGq"
        },
        "identifier": [
          {
            "system": "http://example.org/sampleepisodeofcare-identifier",
            "value": "123"
          }
        ],
        "status": "planned",
        "type": [
          {
            "coding": [
              {
                "system": "http://hl7.org/fhir/episodeofcare-type",
                "code": "hacc",
                "display": "Home and Community Care"
              }
            ]
          }
        ],
        "diagnosis": [
          {
            "condition": {
              "reference": "Condition/88580"
            },
            "role": {
              "coding": [
                {
                  "system": "http://hl7.org/fhir/diagnosis-role",
                  "code": "CC",
                  "display": "Chief complaint"
                }
              ]
            },
            "rank": 1
          }
        ],
        "patient": {
          "reference": "Patient/2562547"
        },
        "managingOrganization": {
          "reference": "Organization/88547"
        },
        "period": {
          "start": "2021-09-14T18:30:00Z",
          "end": "2021-09-15T18:30:00Z"
        }
      },
      "search": {
        "mode": "match"
      }
    },
    {
      "fullUrl": "https://hapi.fhir.org/baseR4/EpisodeOfCare/2559639",
      "resource": {
        "resourceType": "EpisodeOfCare",
        "id": "2559639",
        "meta": {
          "versionId": "1",
          "lastUpdated": "2021-09-21T11:27:44.635+00:00",
          "source": "#apUo4tRmT8H6zJZM"
        },
        "identifier": [
          {
            "system": "http://example.org/sampleepisodeofcare-identifier",
            "value": "123"
          }
        ],
        "status": "onhold",
        "type": [
          {
            "coding": [
              {
                "system": "http://hl7.org/fhir/episodeofcare-type",
                "code": "diab",
                "display": "Post coordinated diabetes program"
              }
            ]
          }
        ],
        "diagnosis": [
          {
            "condition": {
              "reference": "Condition/88580"
            },
            "role": {
              "coding": [
                {
                  "system": "http://hl7.org/fhir/diagnosis-role",
                  "code": "CC",
                  "display": "Chief complaint"
                }
              ]
            },
            "rank": 1
          }
        ],
        "patient": {
          "reference": "Patient/2559637"
        },
        "managingOrganization": {
          "reference": "Organization/88547"
        },
        "period": {
          "start": "2021-09-05T18:30:00Z",
          "end": "2021-09-19T18:30:00Z"
        }
      },
      "search": {
        "mode": "match"
      }
    },
    {
      "fullUrl": "https://hapi.fhir.org/baseR4/EpisodeOfCare/2559638",
      "resource": {
        "resourceType": "EpisodeOfCare",
        "id": "2559638",
        "meta": {
          "versionId": "1",
          "lastUpdated": "2021-09-21T11:27:42.650+00:00",
          "source": "#vrwNncdWAc6JJM3n"
        },
        "identifier": [
          {
            "system": "http://example.org/sampleepisodeofcare-identifier",
            "value": "123"
          }
        ],
        "status": "planned",
        "type": [
          {
            "coding": [
              {
                "system": "http://hl7.org/fhir/episodeofcare-type",
                "code": "cacp",
                "display": "Community-based aged care"
              }
            ]
          }
        ],
        "diagnosis": [
          {
            "condition": {
              "reference": "Condition/88580"
            },
            "role": {
              "coding": [
                {
                  "system": "http://hl7.org/fhir/diagnosis-role",
                  "code": "CC",
                  "display": "Chief complaint"
                }
              ]
            },
            "rank": 1
          }
        ],
        "patient": {
          "reference": "Patient/2559637"
        },
        "managingOrganization": {
          "reference": "Organization/88547"
        },
        "period": {
          "start": "2021-09-18T18:30:00Z",
          "end": "2021-09-20T18:30:00Z"
        }
      },
      "search": {
        "mode": "match"
      }
    },
    {
      "fullUrl": "https://hapi.fhir.org/baseR4/EpisodeOfCare/2559634",
      "resource": {
        "resourceType": "EpisodeOfCare",
        "id": "2559634",
        "meta": {
          "versionId": "1",
          "lastUpdated": "2021-09-21T11:18:54.645+00:00",
          "source": "#Nyh0nElf58ovcx5Z"
        },
        "identifier": [
          {
            "system": "http://example.org/sampleepisodeofcare-identifier",
            "value": "123"
          }
        ],
        "status": "onhold",
        "type": [
          {
            "coding": [
              {
                "system": "http://hl7.org/fhir/episodeofcare-type",
                "code": "diab",
                "display": "100000002"
              }
            ]
          }
        ],
        "diagnosis": [
          {
            "condition": {
              "reference": "Condition/88580"
            },
            "role": {
              "coding": [
                {
                  "system": "http://hl7.org/fhir/diagnosis-role",
                  "code": "CC",
                  "display": "Chief complaint"
                }
              ]
            },
            "rank": 1
          }
        ],
        "patient": {
          "reference": "Patient/2559632"
        },
        "managingOrganization": {
          "reference": "Organization/88547"
        },
        "period": {
          "start": "2021-09-05T18:30:00Z",
          "end": "2021-09-19T18:30:00Z"
        }
      },
      "search": {
        "mode": "match"
      }
    },
    {
      "fullUrl": "https://hapi.fhir.org/baseR4/EpisodeOfCare/2559633",
      "resource": {
        "resourceType": "EpisodeOfCare",
        "id": "2559633",
        "meta": {
          "versionId": "1",
          "lastUpdated": "2021-09-21T11:18:53.491+00:00",
          "source": "#a109WVwLngsIQKIc"
        },
        "identifier": [
          {
            "system": "http://example.org/sampleepisodeofcare-identifier",
            "value": "123"
          }
        ],
        "status": "planned",
        "type": [
          {
            "coding": [
              {
                "system": "http://hl7.org/fhir/episodeofcare-type",
                "code": "cacp",
                "display": "100000004"
              }
            ]
          }
        ],
        "diagnosis": [
          {
            "condition": {
              "reference": "Condition/88580"
            },
            "role": {
              "coding": [
                {
                  "system": "http://hl7.org/fhir/diagnosis-role",
                  "code": "CC",
                  "display": "Chief complaint"
                }
              ]
            },
            "rank": 1
          }
        ],
        "patient": {
          "reference": "Patient/2559632"
        },
        "managingOrganization": {
          "reference": "Organization/88547"
        },
        "period": {
          "start": "2021-09-18T18:30:00Z",
          "end": "2021-09-20T18:30:00Z"
        }
      },
      "search": {
        "mode": "match"
      }
    },
    {
      "fullUrl": "https://hapi.fhir.org/baseR4/EpisodeOfCare/2560328",
      "resource": {
        "resourceType": "EpisodeOfCare",
        "id": "2560328",
        "meta": {
          "versionId": "1",
          "lastUpdated": "2021-09-21T22:07:50.881+00:00",
          "source": "#LwWrLGd6N5xCJoc7"
        },
        "identifier": [
          {
            "system": "http://example.org/sampleepisodeofcare-identifier",
            "value": "123"
          }
        ],
        "status": "active",
        "type": [
          {
            "coding": [
              {
                "system": "http://hl7.org/fhir/episodeofcare-type",
                "code": "pac",
                "display": "Post Acute Care"
              }
            ]
          }
        ],
        "diagnosis": [
          {
            "condition": {
              "reference": "Condition/88580"
            },
            "role": {
              "coding": [
                {
                  "system": "http://hl7.org/fhir/diagnosis-role",
                  "code": "CC",
                  "display": "Chief complaint"
                }
              ]
            },
            "rank": 1
          }
        ],
        "patient": {
          "reference": "Patient/2560327"
        },
        "managingOrganization": {
          "reference": "Organization/88547"
        },
        "period": {
          "start": "2021-08-31T07:00:00Z",
          "end": "2021-09-20T07:00:00Z"
        }
      },
      "search": {
        "mode": "match"
      }
    },
    {
      "fullUrl": "https://hapi.fhir.org/baseR4/EpisodeOfCare/2560290",
      "resource": {
        "resourceType": "EpisodeOfCare",
        "id": "2560290",
        "meta": {
          "versionId": "1",
          "lastUpdated": "2021-09-21T19:52:55.559+00:00",
          "source": "#QwlW8CAsb05ZuxQR"
        },
        "identifier": [
          {
            "system": "http://example.org/sampleepisodeofcare-identifier",
            "value": "123"
          }
        ],
        "status": "active",
        "type": [
          {
            "coding": [
              {
                "system": "http://hl7.org/fhir/episodeofcare-type",
                "code": "pac",
                "display": "Post Acute Care"
              }
            ]
          }
        ],
        "diagnosis": [
          {
            "condition": {
              "reference": "Condition/88580"
            },
            "role": {
              "coding": [
                {
                  "system": "http://hl7.org/fhir/diagnosis-role",
                  "code": "CC",
                  "display": "Chief complaint"
                }
              ]
            },
            "rank": 1
          }
        ],
        "patient": {
          "reference": "Patient/2560289"
        },
        "managingOrganization": {
          "reference": "Organization/88547"
        },
        "period": {
          "start": "2021-09-01T00:00:00Z",
          "end": "2021-09-16T00:00:00Z"
        }
      },
      "search": {
        "mode": "match"
      }
    }
  ]
}
```


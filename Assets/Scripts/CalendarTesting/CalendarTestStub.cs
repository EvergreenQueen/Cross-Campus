using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using GlobalVars;
using Ink.Parsed;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CalendarTestStub : MonoBehaviour
{
    /*
        testing courses/clubs guide: 
            cs141: times: TR afternoons, location: wilcox's office
            phys040B: times: TWR middays, location: physics 2000
            math031: times; MWF evenings, location: belltower

            Gamespawn: times: W afternoons, location: hub
            HLG: times: sat/sun middays and evenings, location: et cetera
            ACM: times: MWF afternoons and evenings, location: wilcox's office
    */

    public Button b_cs141Add; // SET IN I N S P E C T O R 
    public Button b_cs141Remove; // SET IN I N S P E C T O R 
    public Button b_phys040bAdd; // SET IN I N S P E C T O R 
    public Button b_phys040bRemove; // SET IN I N S P E C T O R 
    public Button b_math031Add; // SET IN I N S P E C T O R 
    public Button b_math031Remove; // SET IN I N S P E C T O R 

    public Button b_gamespawnAdd; // SET IN I N S P E C T O R 
    public Button b_gamespawnRemove; // SET IN I N S P E C T O R 
    public Button b_hlgAdd; // SET IN I N S P E C T O R 
    public Button b_hlgRemove; // SET IN I N S P E C T O R 
    public Button b_acmAdd; // SET IN I N S P E C T O R 
    public Button b_acmRemove; // SET IN I N S P E C T O R 
    
    // course stubs
    public Course course_cs141;
    public Course course_phys040b;
    public Course course_math031;

    // club stubs
    public Club club_gamespawn;
    public Club club_hlg;
    public Club club_acm;


    public Calendar calendar;

    public void CalendarAddCourse(Course newCourse)
    {
        calendar.AddCourse(newCourse);
    }

    public void CalendarRemoveCourse(Course oldCourse) 
    {
        calendar.RemoveCourse(oldCourse);
    }

    public void CalendarAddClub(Club newClub)
    {
        calendar.AddClub(newClub);
    }

    public void CalendarRemoveClub(Club oldClub) 
    {
        calendar.RemoveClub(oldClub);
    }

    // Start is called before the first frame update
    void Start()
    {
        // dd = GetComponent<TMP_Dropdown>();
        //dropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(dropdown);}); // dd reference set in inspector
        // button = GetComponent<Button>();

        b_cs141Add.onClick.AddListener(delegate { CalendarAddCourse(course_cs141); });
        b_cs141Remove.onClick.AddListener(delegate { CalendarRemoveCourse(course_cs141); });
        b_phys040bAdd.onClick.AddListener(delegate { CalendarAddCourse(course_phys040b); });
        b_phys040bRemove.onClick.AddListener(delegate { CalendarRemoveCourse(course_phys040b); });
        b_math031Add.onClick.AddListener(delegate { CalendarAddCourse(course_math031); });
        b_math031Remove.onClick.AddListener(delegate { CalendarRemoveCourse(course_math031); });

        b_gamespawnAdd.onClick.AddListener(delegate { CalendarAddClub(club_gamespawn); });
        b_gamespawnRemove.onClick.AddListener(delegate { CalendarRemoveClub(club_gamespawn); });
        b_hlgAdd.onClick.AddListener(delegate { CalendarAddClub(club_hlg); });
        b_hlgRemove.onClick.AddListener(delegate { CalendarRemoveClub(club_hlg); });
        b_acmAdd.onClick.AddListener(delegate { CalendarAddClub(club_acm); });
        b_acmRemove.onClick.AddListener(delegate { CalendarRemoveClub(club_acm); });

        // define stubs
        course_cs141    = new Course("cs141", 
                                    new List<TimeSlot> {TimeSlot.afternoon},
                                    new List<Day> {Day.Tuesday, Day.Thursday},
                                    Location.wilcoxs_office); 

        course_phys040b = new Course("phys040b", 
                                    new List<TimeSlot> {TimeSlot.midday},
                                    new List<Day> {Day.Tuesday, Day.Wednesday, Day.Thursday},
                                    Location.physics_2000);

        course_math031  = new Course("math031", 
                                    new List<TimeSlot> {TimeSlot.evening}, 
                                    new List<Day> {Day.Monday, Day.Wednesday, Day.Friday}, 
                                    Location.belltower);
        
        club_gamespawn  = new Club("Gamespawn",
                                    new List<TimeSlot> {TimeSlot.afternoon},
                                    new List<Day> {Day.Wednesday},
                                    Location.hub);
        
        club_hlg        = new Club("HLG",
                                    new List<TimeSlot> {TimeSlot.midday, TimeSlot.evening},
                                    new List<Day> {Day.Sunday, Day.Saturday},
                                    Location.et_cetera);
        
        club_acm        = new Club("ACM",
                                    new List<TimeSlot> {TimeSlot.afternoon, TimeSlot.evening},
                                    new List<Day> {Day.Monday, Day.Wednesday, Day.Friday},
                                    Location.wilcoxs_office);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}

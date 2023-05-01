using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace be.Models
{
    public class Program_Matching_Criteria
    {
        [Key]
        public string ProgramId { get; set; } = string.Empty;
        public string ProgramName { get; set; } = string.Empty;

        public double Sum = 0;

        public void Fix_Sum()
        {
            PropertyInfo[] properties = typeof(Program_Matching_Criteria).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (!(property.Name == "Sum" || property.Name == "ProgramName" || property.Name == "ProgramId"))
                {
                    this.Sum = 0;
                    this.Sum += (double)property.GetValue(this);
                }
            }
        }

        public void Update_Program(Dictionary<string, double> Values, string ProgramName)
        {
            this.ProgramName = ProgramName;
            PropertyInfo[] properties = typeof(Program_Matching_Criteria).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (!(property.Name == "Sum" || property.Name == "ProgramName" || property.Name == "ProgramId"))
                {
                    if (Values.ContainsKey(property.Name))
                    {
                        double old_value = (double)property.GetValue(this);
                        property.SetValue(this, Values[property.Name]);
                        this.Sum += (Values[property.Name] - old_value);
                    }
                }
            }
        }

        public Program_Matching_Criteria(Dictionary<string, double> Values, string ProgramName)
        {
            this.ProgramName = ProgramName;
            PropertyInfo[] properties = typeof(Program_Matching_Criteria).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (!(property.Name == "Sum" || property.Name == "ProgramName" || property.Name == "ProgramId"))
                {
                    if (Values.ContainsKey(property.Name))
                    {
                        property.SetValue(this, Values[property.Name]);
                        this.Sum+= Values[property.Name];
                    }
                    else
                    {
                        property.SetValue(this, 0);
                    }
                }
            }
        }
        public double Calculate_Score(Program_Matching_Criteria StudentScores)
        {
            double Productsum = 0;
            PropertyInfo[] properties = typeof(Program_Matching_Criteria).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (!(property.Name == "Sum" || property.Name == "ProgramName" || property.Name == "ProgramId"))
                {
                    Productsum += (double)property.GetValue(this) * (double)property.GetValue(StudentScores);
                }

            }
            Productsum /= this.Sum;
            return Productsum;
        }

        public Program_Matching_Criteria()
        {
            Math = 0;
            Physics = 0;
            Chemistry = 0;
            Biology = 0;
            Arabic = 0;
            English = 0;
            third_language = 0;
            Computer_IT = 0;
            Social_Studies_Government_Geography = 0;
            History = 0;
            Philosophy = 0;
            Psychology = 0;
            Art = 0;
            Music = 0;
            Physical_Education = 0;
            Business = 0;
            Accounting = 0;
            Adventure_or_action = 0;
            Animal_care_or_animal_training = 0;
            Art_or_drawing_or_paint_or_sculpture = 0;
            Astronomy_or_weather_forecasting = 0;
            Athletic_activities = 0;
            Aviation = 0;
            Entrepreneurship_or_Jr_Achievement = 0;
            Mentoring_or_Future_Teachers = 0;
            Computer_graphics = 0;
            Computer_programming = 0;
            Computer_social_networking = 0;
            Cooking = 0;
            Dance = 0;
            Debate_or_public_speaking = 0;
            Drama_or_lighting_or_scenery_or_theatre = 0;
            Electronics_or_radio_or_sound_systems_or_television = 0;
            Ethnic_and_regional_cultures_or_history = 0;
            Problem_solving_or_strategy = 0;
            Gardening = 0;
            Healthcare_related_services = 0;
            ROTC_or_other_military_activities = 0;
            Language_club_orethnic_heritage_ = 0;
            Building_or_machining_or_repair_work = 0;
            Math_team = 0;
            Music_band_or_chorus_or_performance = 0;
            Outdoor_activities_or_camping_or_hiking = 0;
            Nutrition_or_exercise_or_physical_fitness = 0;
            Photography_or_video = 0;
            Student_government_or_leadership = 0;
            Religious_training_or_spirituality = 0;
            Science_or_engineering = 0;
            Social_work_service_activities = 0;
            Technology_clubs = 0;
            Travel = 0;
            Writing_or_reading_or_yearbook_or_newspaper = 0;
            Working_Alone = 0;
            Helping_People_Solve_Problems = 0;
            Working_Outdoors = 0;
            Working_With_Immediately_Practical_Things = 0;
            Working_with_Organizations_and_Groups = 0;
            Working_with_Detailed_Logical_Structure = 0;
            Working_With_Children_or_Young_People = 0;
            Study_a_major_that_has_a_clear_connection_to_a_career = 0;
            Exert_power_and_authority = 0;
            Work_on_self_initiated_projects = 0;
            Speak_to_and_facilitate_groups_of_people_at_work = 0;
            Engage_in_high_stress_and_perhaps_risky_undertakings = 0;
            Use_your_manual_and_mechanical_skills = 0;
            Participate_in_sports_and_physically_demanding_activities = 0;
            Maintain_hard_working_disciplined_study_skills = 0;
            Study_different_peoples_and_cultures = 0;
            Engage_in_religious_activities = 0;
            Teach_children_young_people_or_adults = 0;
            Avoid_working_with_hazardous_situations_or_materials = 0;
            Engage_in_social_issues_and_actions = 0;
            Stay_involved_in_current_events = 0;
            Rely_on_mental_images = 0;
            Engage_in_scientific_problem_solving = 0;
            Public_dramatic_or_musical_performance = 0;
            Exercise_your_creativity = 0;
            Limit_choices_to_careers_with_an_above_average_salary = 0;
            Avoid_majors_lasting_more_than_4_years = 0;
            Assist_others_to_resolve_conflict_situations = 0;
            Work_in_a_business_environment = 0;
            Combine_work_with_learning = 0;
            Value_leadership_training_and_experiences = 0;
            Rely_on_and_trust_in_your_peers_camaraderie = 0;
            Provide_nursing_andor_healthcare_related_services = 0;
        }

        public double Math { get; set; }
        public double Physics { get; set; }
        public double Chemistry { get; set; }
        public double Biology { get; set; }
        public double Arabic { get; set; }
        public double English { get; set; }
        public double third_language { get; set; }
        public double Computer_IT { get; set; }
        public double Social_Studies_Government_Geography { get; set; }
        public double History { get; set; }
        public double Philosophy { get; set; }
        public double Psychology { get; set; }
        public double Art { get; set; }
        public double Music { get; set; }
        public double Physical_Education { get; set; }
        public double Business { get; set; }
        public double Accounting { get; set; }
        public double Adventure_or_action { get; set; }
        public double Animal_care_or_animal_training { get; set; }
        public double Art_or_drawing_or_paint_or_sculpture { get; set; }
        public double Astronomy_or_weather_forecasting { get; set; }
        public double Athletic_activities { get; set; }
        public double Aviation { get; set; }
        public double Entrepreneurship_or_Jr_Achievement { get; set; }
        public double Mentoring_or_Future_Teachers { get; set; }
        public double Computer_graphics { get; set; }
        public double Computer_programming { get; set; }
        public double Computer_social_networking { get; set; }
        public double Cooking { get; set; }
        public double Dance { get; set; }
        public double Debate_or_public_speaking { get; set; }
        public double Drama_or_lighting_or_scenery_or_theatre { get; set; }
        public double Electronics_or_radio_or_sound_systems_or_television { get; set; }
        public double Ethnic_and_regional_cultures_or_history { get; set; }
        public double Problem_solving_or_strategy { get; set; }
        public double Gardening { get; set; }
        public double Healthcare_related_services { get; set; }
        public double ROTC_or_other_military_activities { get; set; }
        public double Language_club_orethnic_heritage_ { get; set; }
        public double Building_or_machining_or_repair_work { get; set; }
        public double Math_team { get; set; }
        public double Music_band_or_chorus_or_performance { get; set; }
        public double Outdoor_activities_or_camping_or_hiking { get; set; }
        public double Nutrition_or_exercise_or_physical_fitness { get; set; }
        public double Photography_or_video { get; set; }
        public double Student_government_or_leadership { get; set; }
        public double Religious_training_or_spirituality { get; set; }
        public double Science_or_engineering { get; set; }
        public double Social_work_service_activities { get; set; }
        public double Technology_clubs { get; set; }
        public double Travel { get; set; }
        public double Writing_or_reading_or_yearbook_or_newspaper { get; set; }
        public double Working_Alone { get; set; }
        public double Helping_People_Solve_Problems { get; set; }
        public double Working_Outdoors { get; set; }
        public double Working_With_Immediately_Practical_Things { get; set; }
        public double Working_with_Organizations_and_Groups { get; set; }
        public double Working_with_Detailed_Logical_Structure { get; set; }
        public double Working_With_Children_or_Young_People { get; set; }
        public double Study_a_major_that_has_a_clear_connection_to_a_career { get; set; }
        public double Exert_power_and_authority { get; set; }
        public double Work_on_self_initiated_projects { get; set; }
        public double Speak_to_and_facilitate_groups_of_people_at_work { get; set; }
        public double Engage_in_high_stress_and_perhaps_risky_undertakings { get; set; }
        public double Use_your_manual_and_mechanical_skills { get; set; }
        public double Participate_in_sports_and_physically_demanding_activities { get; set; }
        public double Maintain_hard_working_disciplined_study_skills { get; set; }
        public double Study_different_peoples_and_cultures { get; set; }
        public double Engage_in_religious_activities { get; set; }
        public double Teach_children_young_people_or_adults { get; set; }
        public double Avoid_working_with_hazardous_situations_or_materials { get; set; }
        public double Engage_in_social_issues_and_actions { get; set; }
        public double Stay_involved_in_current_events { get; set; }
        public double Rely_on_mental_images { get; set; }
        public double Engage_in_scientific_problem_solving { get; set; }
        public double Public_dramatic_or_musical_performance { get; set; }
        public double Exercise_your_creativity { get; set; }
        public double Limit_choices_to_careers_with_an_above_average_salary { get; set; }
        public double Avoid_majors_lasting_more_than_4_years { get; set; }
        public double Assist_others_to_resolve_conflict_situations { get; set; }
        public double Work_in_a_business_environment { get; set; }
        public double Combine_work_with_learning { get; set; }
        public double Value_leadership_training_and_experiences { get; set; }
        public double Rely_on_and_trust_in_your_peers_camaraderie { get; set; }
        public double Provide_nursing_andor_healthcare_related_services { get; set; }

        

    }
}

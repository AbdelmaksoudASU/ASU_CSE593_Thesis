using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace be.Migrations
{
    /// <inheritdoc />
    public partial class FixSumModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuizDB",
                columns: table => new
                {
                    ProgramId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProgramName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sum = table.Column<double>(type: "float", nullable: false),
                    Math = table.Column<double>(type: "float", nullable: false),
                    Physics = table.Column<double>(type: "float", nullable: false),
                    Chemistry = table.Column<double>(type: "float", nullable: false),
                    Biology = table.Column<double>(type: "float", nullable: false),
                    Arabic = table.Column<double>(type: "float", nullable: false),
                    English = table.Column<double>(type: "float", nullable: false),
                    third_language = table.Column<double>(type: "float", nullable: false),
                    Computer_IT = table.Column<double>(type: "float", nullable: false),
                    Social_Studies_Government_Geography = table.Column<double>(type: "float", nullable: false),
                    History = table.Column<double>(type: "float", nullable: false),
                    Philosophy = table.Column<double>(type: "float", nullable: false),
                    Psychology = table.Column<double>(type: "float", nullable: false),
                    Art = table.Column<double>(type: "float", nullable: false),
                    Music = table.Column<double>(type: "float", nullable: false),
                    Physical_Education = table.Column<double>(type: "float", nullable: false),
                    Business = table.Column<double>(type: "float", nullable: false),
                    Accounting = table.Column<double>(type: "float", nullable: false),
                    Adventure_or_action = table.Column<double>(type: "float", nullable: false),
                    Animal_care_or_animal_training = table.Column<double>(type: "float", nullable: false),
                    Art_or_drawing_or_paint_or_sculpture = table.Column<double>(type: "float", nullable: false),
                    Astronomy_or_weather_forecasting = table.Column<double>(type: "float", nullable: false),
                    Athletic_activities = table.Column<double>(type: "float", nullable: false),
                    Aviation = table.Column<double>(type: "float", nullable: false),
                    Entrepreneurship_or_Jr_Achievement = table.Column<double>(type: "float", nullable: false),
                    Mentoring_or_Future_Teachers = table.Column<double>(type: "float", nullable: false),
                    Computer_graphics = table.Column<double>(type: "float", nullable: false),
                    Computer_programming = table.Column<double>(type: "float", nullable: false),
                    Computer_social_networking = table.Column<double>(type: "float", nullable: false),
                    Cooking = table.Column<double>(type: "float", nullable: false),
                    Dance = table.Column<double>(type: "float", nullable: false),
                    Debate_or_public_speaking = table.Column<double>(type: "float", nullable: false),
                    Drama_or_lighting_or_scenery_or_theatre = table.Column<double>(type: "float", nullable: false),
                    Electronics_or_radio_or_sound_systems_or_television = table.Column<double>(type: "float", nullable: false),
                    Ethnic_and_regional_cultures_or_history = table.Column<double>(type: "float", nullable: false),
                    Problem_solving_or_strategy = table.Column<double>(type: "float", nullable: false),
                    Gardening = table.Column<double>(type: "float", nullable: false),
                    Healthcare_related_services = table.Column<double>(type: "float", nullable: false),
                    ROTC_or_other_military_activities = table.Column<double>(type: "float", nullable: false),
                    Language_club_orethnic_heritage_ = table.Column<double>(type: "float", nullable: false),
                    Building_or_machining_or_repair_work = table.Column<double>(type: "float", nullable: false),
                    Math_team = table.Column<double>(type: "float", nullable: false),
                    Music_band_or_chorus_or_performance = table.Column<double>(type: "float", nullable: false),
                    Outdoor_activities_or_camping_or_hiking = table.Column<double>(type: "float", nullable: false),
                    Nutrition_or_exercise_or_physical_fitness = table.Column<double>(type: "float", nullable: false),
                    Photography_or_video = table.Column<double>(type: "float", nullable: false),
                    Student_government_or_leadership = table.Column<double>(type: "float", nullable: false),
                    Religious_training_or_spirituality = table.Column<double>(type: "float", nullable: false),
                    Science_or_engineering = table.Column<double>(type: "float", nullable: false),
                    Social_work_service_activities = table.Column<double>(type: "float", nullable: false),
                    Technology_clubs = table.Column<double>(type: "float", nullable: false),
                    Travel = table.Column<double>(type: "float", nullable: false),
                    Writing_or_reading_or_yearbook_or_newspaper = table.Column<double>(type: "float", nullable: false),
                    Working_Alone = table.Column<double>(type: "float", nullable: false),
                    Helping_People_Solve_Problems = table.Column<double>(type: "float", nullable: false),
                    Working_Outdoors = table.Column<double>(type: "float", nullable: false),
                    Working_With_Immediately_Practical_Things = table.Column<double>(type: "float", nullable: false),
                    Working_with_Organizations_and_Groups = table.Column<double>(type: "float", nullable: false),
                    Working_with_Detailed_Logical_Structure = table.Column<double>(type: "float", nullable: false),
                    Working_With_Children_or_Young_People = table.Column<double>(type: "float", nullable: false),
                    Study_a_major_that_has_a_clear_connection_to_a_career = table.Column<double>(type: "float", nullable: false),
                    Exert_power_and_authority = table.Column<double>(type: "float", nullable: false),
                    Work_on_self_initiated_projects = table.Column<double>(type: "float", nullable: false),
                    Speak_to_and_facilitate_groups_of_people_at_work = table.Column<double>(type: "float", nullable: false),
                    Engage_in_high_stress_and_perhaps_risky_undertakings = table.Column<double>(type: "float", nullable: false),
                    Use_your_manual_and_mechanical_skills = table.Column<double>(type: "float", nullable: false),
                    Participate_in_sports_and_physically_demanding_activities = table.Column<double>(type: "float", nullable: false),
                    Maintain_hard_working_disciplined_study_skills = table.Column<double>(type: "float", nullable: false),
                    Study_different_peoples_and_cultures = table.Column<double>(type: "float", nullable: false),
                    Engage_in_religious_activities = table.Column<double>(type: "float", nullable: false),
                    Teach_children_young_people_or_adults = table.Column<double>(type: "float", nullable: false),
                    Avoid_working_with_hazardous_situations_or_materials = table.Column<double>(type: "float", nullable: false),
                    Engage_in_social_issues_and_actions = table.Column<double>(type: "float", nullable: false),
                    Stay_involved_in_current_events = table.Column<double>(type: "float", nullable: false),
                    Rely_on_mental_images = table.Column<double>(type: "float", nullable: false),
                    Engage_in_scientific_problem_solving = table.Column<double>(type: "float", nullable: false),
                    Public_dramatic_or_musical_performance = table.Column<double>(type: "float", nullable: false),
                    Exercise_your_creativity = table.Column<double>(type: "float", nullable: false),
                    Limit_choices_to_careers_with_an_above_average_salary = table.Column<double>(type: "float", nullable: false),
                    Avoid_majors_lasting_more_than_4_years = table.Column<double>(type: "float", nullable: false),
                    Assist_others_to_resolve_conflict_situations = table.Column<double>(type: "float", nullable: false),
                    Work_in_a_business_environment = table.Column<double>(type: "float", nullable: false),
                    Combine_work_with_learning = table.Column<double>(type: "float", nullable: false),
                    Value_leadership_training_and_experiences = table.Column<double>(type: "float", nullable: false),
                    Rely_on_and_trust_in_your_peers_camaraderie = table.Column<double>(type: "float", nullable: false),
                    Provide_nursing_andor_healthcare_related_services = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizDB", x => x.ProgramId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuizDB");
        }
    }
}

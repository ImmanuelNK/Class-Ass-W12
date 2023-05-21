using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;



namespace Week_12_connect_with_SQL_reader
{
    public partial class Form1 : Form
    {
       MySqlConnection sqlConnection;
        MySqlCommand sqlcommand;
        MySqlDataAdapter sqlDataAdapter;
        DataTable dtteam = new DataTable();
        DataTable dtnat = new DataTable();
        DataTable dtteam2 = new DataTable();
        DataTable dtteam3 = new DataTable();
        DataTable dtmanager = new DataTable();
        DataTable dtmanager0 = new DataTable();
        DataTable dtplayer = new DataTable();
        string connectionstring;
        string sqlquery;
        string idpilih = "";
        string command = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            connectionstring = "server=localhost;user=root;pwd=Noel1517;database=premier_league";
            sqlConnection = new MySqlConnection(connectionstring);
            sqlquery = "SELECT team_name as name, team_id as id FROM team ;";
            sqlcommand = new MySqlCommand(sqlquery, sqlConnection);
            sqlDataAdapter = new MySqlDataAdapter(sqlcommand);
            sqlDataAdapter.Fill(dtteam);
            sqlDataAdapter.Fill(dtteam2);
            sqlDataAdapter.Fill(dtteam3);

            comboBoxTeam.DataSource = dtteam;
            comboBoxTeamManager.DataSource = dtteam2;
            comboBoxTeamPlayer.DataSource = dtteam3;
            comboBoxTeam.ValueMember = "id";
            comboBoxTeamManager.ValueMember = "id";
            comboBoxTeamPlayer.ValueMember = "id";
            comboBoxTeam.DisplayMember = "name";
            comboBoxTeamManager.DisplayMember = "name";
            comboBoxTeamPlayer.DisplayMember = "name";


            sqlquery = "SELECT nation, nationality_id FROM nationality ;";
            sqlcommand = new MySqlCommand(sqlquery, sqlConnection);
            sqlDataAdapter = new MySqlDataAdapter(sqlcommand);
            sqlDataAdapter.Fill(dtnat);
            comboBoxNat.DataSource = dtnat;
            comboBoxNat.ValueMember = "nationality_id";
            comboBoxNat.DisplayMember = "nation";

        }

        private void comboBoxNat_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            command = $"insert into player values('{textBoxID.Text}','{textBoxNum.Text}','{textBoxName.Text}','{comboBoxNat.SelectedValue.ToString()}','{textBoxPos.Text}','{textBoxHeight.Text}','{textBoxWeight.Text}','{dateTimePickerBirth.Value.ToString("yyyy/MM/dd")}','{comboBoxTeam.SelectedValue.ToString()}','1','0')";
            sqlConnection.Open();
            sqlcommand = new MySqlCommand(command, sqlConnection);
            sqlcommand.ExecuteNonQuery();
            sqlConnection.Close();

        }

        private void comboBoxTeamManager_SelectedIndexChanged(object sender, EventArgs e)
        {
            dtmanager = new DataTable();
            dtmanager0 = new DataTable();

            sqlquery = "select m.manager_name, m.manager_id, t.team_name, t.team_id, m.birthdate, n.nation from manager m, nationality n , team t where m.manager_id = t.manager_id AND m.nationality_id = n.nationality_id AND t.team_id ='" + comboBoxTeamManager.SelectedValue.ToString() + "' ;";
            sqlcommand = new MySqlCommand(sqlquery, sqlConnection);
            sqlDataAdapter = new MySqlDataAdapter(sqlcommand);
            sqlDataAdapter.Fill(dtmanager);
            dataGridViewTopLeft.DataSource = dtmanager;

            sqlquery = "select m.manager_name, m.manager_id ,  m.birthdate, n.nation, m.working from manager m, nationality n where m.working = 0 AND m.nationality_id = n.nationality_id ;";
            sqlcommand = new MySqlCommand(sqlquery, sqlConnection);
            sqlDataAdapter = new MySqlDataAdapter(sqlcommand);
            sqlDataAdapter.Fill(dtmanager0);
            dataGridViewBotLeft.DataSource = dtmanager0;
            idteam = comboBoxTeamManager.SelectedValue.ToString();
        }
        string idteam = "";
        private void buttonUpdateManag_Click(object sender, EventArgs e)
        {
            sqlConnection.Open();


            sqlquery = "update manager set working = 1 where manager_id = '" + idmanager + "'; ";
            sqlcommand = new MySqlCommand(sqlquery, sqlConnection);
            sqlcommand.ExecuteNonQuery();

            sqlquery = "update manager set working = 0 where manager_id = '" + dtmanager.Rows[0][1].ToString() + "'; ";
            sqlcommand = new MySqlCommand(sqlquery, sqlConnection);
            sqlcommand.ExecuteNonQuery();

            sqlquery = "UPDATE team set manager_id =  '" + idmanager + "' where team_id = '" + idteam + "'; ";
            sqlcommand = new MySqlCommand(sqlquery, sqlConnection);
            sqlcommand.ExecuteNonQuery();

            dtmanager = new DataTable();
            dtmanager0 = new DataTable();

            sqlquery = "select m.manager_name, m.manager_id, t.team_name, t.team_id, m.birthdate, n.nation from manager m, nationality n , team t where m.manager_id = t.manager_id AND m.nationality_id = n.nationality_id AND t.team_id ='" + comboBoxTeamManager.SelectedValue.ToString() + "' ;";
            sqlcommand = new MySqlCommand(sqlquery, sqlConnection);
            sqlDataAdapter = new MySqlDataAdapter(sqlcommand);
            sqlDataAdapter.Fill(dtmanager);
            dataGridViewTopLeft.DataSource = dtmanager;

            sqlquery = "select m.manager_name, m.manager_id ,  m.birthdate, n.nation, m.working from manager m, nationality n where m.working = 0 AND m.nationality_id = n.nationality_id ;";
            sqlcommand = new MySqlCommand(sqlquery, sqlConnection);
            sqlDataAdapter = new MySqlDataAdapter(sqlcommand);
            sqlDataAdapter.Fill(dtmanager0);
            dataGridViewBotLeft.DataSource = dtmanager0;
            

            sqlConnection.Close();
        }

        string idmanager = "";
        private void dataGridViewBotLeft_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            idmanager = dtmanager0.Rows[e.RowIndex][1].ToString(); 
        }

        private void comboBoxTeamPlayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            dtplayer.Clear();

            sqlquery = " select p.player_id, p.player_name,n.nation,p.playing_pos,p.team_number, p.height, p.weight, p.birthdate, p.status from player p, nationality n where p.team_id = '" + comboBoxTeamPlayer.SelectedValue.ToString() + "' AND n.nationality_id = p.nationality_id AND p.status = '1';";
            sqlcommand = new MySqlCommand(sqlquery, sqlConnection);
            sqlDataAdapter = new MySqlDataAdapter(sqlcommand);
            sqlDataAdapter.Fill(dtplayer);
            dataGridViewRight.DataSource = dtplayer;

        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if ( dtplayer.Rows.Count <= 11)
            {
                MessageBox.Show("JANGAN DELETE LAGI");
            }    
            else
            {
                command = $"update player set status = '0' where player_id = '{idpilih}';";
                sqlConnection.Open();
                sqlcommand = new MySqlCommand(command, sqlConnection);
                sqlcommand.ExecuteNonQuery();
                dtplayer.Clear();
                sqlquery = " select p.player_id, p.player_name,n.nation,p.playing_pos,p.team_number, p.height, p.weight, p.birthdate, p.status from player p, nationality n where p.team_id = '" + comboBoxTeamPlayer.SelectedValue.ToString() + "' AND n.nationality_id = p.nationality_id AND p.status = '1';";
                sqlConnection = new MySqlConnection(connectionstring);
                sqlcommand = new MySqlCommand(sqlquery, sqlConnection);
                sqlDataAdapter = new MySqlDataAdapter(sqlcommand);
                sqlDataAdapter.Fill(dtplayer);
                dataGridViewRight.DataSource = dtplayer;
                sqlConnection.Close();
            }
           
        }

        private void dataGridViewRight_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            idpilih = dtplayer.Rows[e.RowIndex][0].ToString();
        }
    }
    
}

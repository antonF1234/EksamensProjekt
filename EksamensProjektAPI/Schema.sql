-- Drop table if exists else create them
DROP TABLE IF EXISTS tasks_skills CASCADE;
DROP TABLE IF EXISTS users_skills CASCADE;
DROP TABLE IF EXISTS users_projects CASCADE;
DROP TABLE IF EXISTS tasks CASCADE;
DROP TABLE IF EXISTS skills CASCADE;
DROP TABLE IF EXISTS projects CASCADE;
DROP TABLE IF EXISTS users CASCADE;
DROP TABLE IF EXISTS time_recording CASCADE;

-- USERS
CREATE TABLE users (
                       user_id     SERIAL PRIMARY KEY,
                       username    VARCHAR(50) NOT NULL UNIQUE,
                       password    VARCHAR(255) NOT NULL,
                       email       VARCHAR(255) NOT NULL UNIQUE,
                       is_admin    BOOLEAN NOT NULL DEFAULT FALSE
);


-- PROJECTS
CREATE TABLE projects (
                          project_id  SERIAL PRIMARY KEY,
                          name        TEXT NOT NULL,
                          description TEXT,
                          deadline    DATE,
                          status      VARCHAR(20)
);

-- USERS_PROJECTS (many-to-many)
CREATE TABLE users_projects (
                                user_project SERIAL PRIMARY KEY,
                                user_id      INT REFERENCES users(user_id) ON DELETE CASCADE,
                                project_id   INT REFERENCES projects(project_id) ON DELETE CASCADE
);

-- index for fast lookup
CREATE INDEX idx_users_projects_user ON users_projects(user_id);
CREATE INDEX idx_users_projects_project ON users_projects(project_id);


-- TASKS
CREATE TABLE tasks (
                       task_id          SERIAL PRIMARY KEY,
                       name             TEXT NOT NULL,
                       start_date       DATE,
                       deadline         DATE,
                       completion_date  DATE,
                       status           VARCHAR(20),
                       project_id       INT NOT NULL REFERENCES projects(project_id) ON DELETE CASCADE
);

-- SKILLS
CREATE TABLE skills (
                        skill_id SERIAL PRIMARY KEY,
                        name     TEXT NOT NULL UNIQUE
);


-- USERS_SKILLS (many-to-many)
CREATE TABLE users_skills (
                              employee_skill_id SERIAL PRIMARY KEY,
                              employee_id       INT REFERENCES users(user_id) ON DELETE CASCADE,
                              skill_id          INT REFERENCES skills(skill_id) ON DELETE CASCADE
);

CREATE INDEX idx_users_skills_user ON users_skills(employee_id);
CREATE INDEX idx_users_skills_skill ON users_skills(skill_id);


CREATE TABLE tasks_skills (
                              task_skill_id SERIAL PRIMARY KEY,
                              task_id       INT REFERENCES tasks(task_id) ON DELETE CASCADE,
                              skill_id      INT REFERENCES skills(skill_id) ON DELETE CASCADE,
                              UNIQUE(task_id, skill_id)
);

CREATE INDEX idx_tasks_skills_task ON tasks_skills(task_id);
CREATE INDEX idx_tasks_skills_skill ON tasks_skills(skill_id);

-- USERS_TASKS (many-to-many)
CREATE TABLE users_tasks (
                             user_task_id SERIAL PRIMARY KEY,
                             user_id      INT REFERENCES users(user_id),
                             task_id      INT REFERENCES tasks(task_id),
                             UNIQUE (user_id, task_id)
);

CREATE INDEX idx_users_tasks_user ON users_tasks(user_id);
CREATE INDEX idx_users_tasks_task ON users_tasks(task_id);

-- TIME RECORDING
CREATE TABLE time_recording (
                                time_record_id    SERIAL PRIMARY KEY,
                                user_id           INT NOT NULL REFERENCES users(user_id) ON DELETE CASCADE,
                                task_id           INT NOT NULL REFERENCES tasks(task_id) ON DELETE CASCADE,
                                start_time        TIMESTAMP,
                                end_time          TIMESTAMP,
                                sum_of_time_second INT
);

CREATE INDEX idx_time_recording_user ON time_recording(user_id);
CREATE INDEX idx_time_recording_task ON time_recording(task_id);

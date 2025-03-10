/*pipeline {
    agent any

    environment {
        REPO_URL = 'https://github.com/AnthonyDiazCabanillas/hospital.git'
        BRANCH = 'main'
        DEPLOY_DIR = 'D:/hospital-build' // Carpeta en el disco D:
    }

    stages {
        stage('Clonar repositorio') {
            steps {
                echo 'Clonando el repositorio...'
                git branch: "${BRANCH}", url: "${REPO_URL}"
            }
        }

        stage('Instalar dependencias') {
            steps {
                echo 'Instalando dependencias...'
                sh 'npm install' // Instala dependencias de Node.js
            }
        }

        stage('Compilar proyecto') {
            steps {
                echo 'Compilando el proyecto...'
                sh 'npm run build' // Compila el proyecto
            }
        }

        stage('Publicar en disco local') {
            steps {
                echo 'Publicando los archivos en el disco D:...'
                bat 'xcopy /E /I /Y "C:\\ProgramData\\Jenkins\\.jenkins\\workspace\\Hospital\\dist" "D:\\hospital-build"'
            }
        }
    }

    post {
        success {
            echo 'El proyecto se ha compilado y publicado correctamente en el disco D:.'
        }
        failure {
            echo 'Hubo un error en la compilación o publicación del proyecto.'
        }
    }
}*/
pipeline {
    agent any

    environment {
        REPO_URL = 'https://github.com/AnthonyDiazCabanillas/hospital.git'
        BRANCH = 'main'
        DEPLOY_DIR = 'D:/hospital-build' // Carpeta en el disco D:
    }

    stages {
        stage('Clonar repositorio') {
            steps {
                echo 'Clonando el repositorio...'
                git branch: "${BRANCH}", url: "${REPO_URL}"
            }
        }

        stage('Instalar dependencias') {
            steps {
                echo 'Instalando dependencias...'
                sh 'npm install' // Instala dependencias de Node.js
            }
        }

        stage('Compilar proyecto') {
            steps {
                echo 'Compilando el proyecto...'
                sh 'npm run build' // Compila el proyecto
            }
        }

        stage('Publicar en disco local') {
            steps {
                echo 'Publicando los archivos en el disco D:...'
                bat '''
                    if not exist "${DEPLOY_DIR}" (
                        mkdir "${DEPLOY_DIR}"
                    )
                    xcopy /E /I /Y "C:\\ProgramData\\Jenkins\\.jenkins\\workspace\\Hospital\\dist\\*" "${DEPLOY_DIR}"
                '''
            }
        }
    }

    post {
        success {
            echo 'El proyecto se ha compilado y publicado correctamente en el disco D:.'
        }
        failure {
            echo 'Hubo un error en la compilación o publicación del proyecto.'
        }
    }
}
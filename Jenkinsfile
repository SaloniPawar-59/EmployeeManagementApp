pipeline {
    agent { label 'dotnet-agent' }  // Runs on Jenkins Agent

    environment {
        AWS_REGION = "ap-south-1"
        S3_BUCKET = "employee-management-artifacts-ap-south-1"   // change this to your bucket
        APP_NAME = "EmployeeManagementApp"
        ENV_NAME = "EmployeeManagementApp-dev"
        DOTNET_ROOT = "/usr/share/dotnet"
    }

    stages {
        stage('Checkout') {
            steps {
                git branch: 'main', url: 'https://github.com/SaloniPawar-59/EmployeeManagementApp.git'
            }
        }

        stage('Restore Dependencies') {
            steps {
                sh 'dotnet restore EmployeeManagementApp/EmployeeManagementApp.csproj'
            }
        }

        stage('Build') {
            steps {
                sh 'dotnet build EmployeeManagementApp/EmployeeManagementApp.csproj -c Release'
            }
        }

        stage('Publish') {
            steps {
                sh 'dotnet publish EmployeeManagementApp/EmployeeManagementApp.csproj -c Release -o published/'
            }
        }

        stage('Package for Elastic Beanstalk') {
            steps {
                sh '''
                cd published
                zip -r ../app.zip .
                cd ..
                aws s3 cp app.zip s3://$S3_BUCKET/app-$(date +%Y%m%d%H%M%S).zip --region $AWS_REGION
                '''
            }
        }

        stage('Deploy to Elastic Beanstalk') {
            steps {
                sh '''
                VERSION_LABEL="app-$(date +%Y%m%d%H%M%S)"
                aws elasticbeanstalk create-application-version \
                    --application-name $APP_NAME \
                    --version-label $VERSION_LABEL \
                    --source-bundle S3Bucket=$S3_BUCKET,S3Key=app.zip \
                    --region $AWS_REGION
                aws elasticbeanstalk update-environment \
                    --application-name $APP_NAME \
                    --environment-name $ENV_NAME \
                    --version-label $VERSION_LABEL \
                    --region $AWS_REGION
                '''
            }
        }
    }

    post {
        success {
            echo "✅ Deployment successful!"
        }
        failure {
            echo "❌ Deployment failed!"
        }
    }
}

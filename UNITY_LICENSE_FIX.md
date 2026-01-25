# Unity Login Issue Fix

## Problem Description

The GitHub Actions workflow was attempting to log in to Unity on every build, resulting in 7 consecutive login failures that triggered a temporary Unity ID lockout.

## Root Cause

The previous workflow performed the following actions on every build:

1. Unity license activation step always attempted to log in using `UNITY_EMAIL` and `UNITY_PASSWORD`
2. Attempted re-activation even when a valid license already existed
3. Multiple builds resulted in repeated login attempts
4. Unity servers detected this as abnormal activity and locked the account

## Solution

The workflow has been modified to work as follows:

### Updated Logic

1. **Check License Status First**
   - Run Unity to verify if the current license is valid before attempting activation
   - Check the log file for "No valid Unity Editor license found" errors

2. **Conditional Activation**
   - If a valid license already exists: Skip activation and display "Skipping re-activation" message
   - If no valid license exists: Attempt activation using `UNITY_EMAIL` and `UNITY_PASSWORD`

3. **Re-verify After Activation**
   - If activation was attempted, verify license status again to confirm success

### Key Changes

#### Windows (PowerShell)
```powershell
# Check license status first
$licenseValid = $false
if (license test succeeds) {
    $licenseValid = $true
    Write-Host "Unity license is already active. Skipping re-activation."
}

# Only attempt activation if no valid license exists
if (-not $licenseValid -and $unityEmail -and $unityPassword) {
    # Run activation logic
}
```

#### macOS (Bash)
```bash
# Check license status first
LICENSE_VALID=false
if (license test succeeds); then
    LICENSE_VALID=true
    echo "Unity license is already active. Skipping re-activation."
fi

# Only attempt activation if no valid license exists
if [ "$LICENSE_VALID" != "true" ] && [ -n "$UNITY_EMAIL" ] && [ -n "$UNITY_PASSWORD" ]; then
    # Run activation logic
fi
```

## Expected Benefits

1. **Reduced Login Attempts**: No unnecessary logins when a license already exists
2. **Prevent Account Lockout**: Eliminates the repeated login issue causing account lockouts
3. **Faster Builds**: Skipping unnecessary activation steps reduces build time
4. **Improved Stability**: License is activated once on the first build, then reused for subsequent builds

## Additional Recommendations

1. **Wait for Account Unlock**: If your account is currently locked, wait 10 minutes as specified in the email before trying again

2. **Verify Secrets**: Ensure `UNITY_EMAIL` and `UNITY_PASSWORD` secrets are correctly configured
   - Navigate to: GitHub Repository → Settings → Secrets and variables → Actions
   - Update if credentials are incorrect

3. **Manual License Activation Option**: If Unity is manually activated on the self-hosted runner, you don't need to set the secrets
   - Manually activated licenses will be automatically recognized by the workflow

## Testing the Fix

To test these changes:

1. Push changes to the main branch
2. GitHub Actions workflow will run automatically
3. Check the logs for these messages:
   - First build: "Activating Unity license with EMAIL/PASSWORD..."
   - Subsequent builds: "Unity license is already active. Skipping re-activation."

## Support

If issues persist or you need additional help:
- Report the issue on GitHub Issues
- Contact Unity Support at support.unity3d.com
